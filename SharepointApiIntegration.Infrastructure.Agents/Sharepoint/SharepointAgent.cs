using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using SharepointApiIIntegration.Domain.Interfaces.Agents;
using SharepointApiIIntegration.Domain.Model.Settings;
using SharepointApiIIntegration.Domain.Model.Sharepoint;

namespace SharepointApiIntegration.Infrastructure.GraphService.Sharepoint;

public class SharepointAgent : ISharepointAgent
{
    private readonly IOptions<ApiSettings> _apiSettingsOptions;
    private readonly ILogger<SharepointAgent> _logger;

    public SharepointAgent(IOptions<ApiSettings> apiSettingsOptions, ILogger<SharepointAgent> logger)
    {
        _apiSettingsOptions = apiSettingsOptions;
        _logger = logger;
    }

    private async Task<string> GetAuthTokenAsync()
    {
        var authUrl = "https://accounts.accesscontrol.windows.net/305da883-09e0-43e2-aa5c-ad85d4d11078/tokens/OAuth/2";
        var token = await Policy
            .Handle<FlurlHttpException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
            .ExecuteAsync(() =>
                authUrl
                    .WithHeader("Accept", "*/*")
                    .WithHeader("ContentType", "application/x-www-form-urlencoded")
                    .WithTimeout(5)
                    .PostUrlEncodedAsync(new
                    {
                        grant_type = "client_credentials",
                        client_id = _apiSettingsOptions.Value.SharepointCredentials.ClientId,
                        client_secret = _apiSettingsOptions.Value.SharepointCredentials.ClientSecret,
                        resource = "00000003-0000-0ff1-ce00-000000000000/lucasas1.sharepoint.com@305da883-09e0-43e2-aa5c-ad85d4d11078"
                    })
                    .ReceiveJson<SPOAuthTokenResponse>()
            );
        
        return token.access_token;
    }

    public async Task PostRequestAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<byte[]> GetFileAttachmentAsync(string fileId)
    {
        try
        {
            var token = await GetAuthTokenAsync();

            var items = await Policy
                .Handle<FlurlHttpException>()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
                .ExecuteAsync(() =>
                    string.Concat(
                            _apiSettingsOptions.Value.SharepointCredentials.SharepointSite,
                            "_api/web/Lists('b4b42c37-22f1-4d95-96cc-8201863415df')/items")
                        .WithHeader("Accept", "application/json;odata=verbose")
                        .WithHeader("Authorization", string.Concat("Bearer ", token))
                        .SetQueryParam("$select", "Title,Attachments,AttachmentFiles")
                        .SetQueryParam("$expand", "AttachmentFiles")
                        .SetQueryParam("$filter", $"ID eq {fileId}")
                        .GetAsync()
                ).ReceiveJson<ListItemsResponse>();

            var listItems = new List<byte[]>();

            foreach (var item in items.Response.ListItems)
            {
                foreach(var attachment in item.AttachmentFiles.results)
                {
                    var downloadedFile = await Policy
                        .Handle<FlurlHttpException>()
                        .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
                        .ExecuteAsync(() =>
                            attachment.__metadata.uri
                                .WithHeader("Authorization", string.Concat("Bearer ", token))
                                .AppendPathSegment("$value")
                                .GetAsync()
                        ).ReceiveBytes();
                    
                    listItems.Add(downloadedFile);
                }
            }

            return listItems.FirstOrDefault();
        }
        catch(FlurlHttpException ex)
        {
            _logger.LogError(ex.Message);
            throw ex;
        }
    }
}