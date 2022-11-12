using System.Net.Http.Headers;
using Flurl.Http;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Polly;
using SharepointApiIIntegration.Domain.Interfaces.Agents;
using SharepointApiIIntegration.Domain.Model.Settings;

namespace SharepointApiIntegration.Infrastructure.GraphService.Sharepoint;

public class SharepointGraphAgent : ISharepointAgent
{
    private readonly IOptions<ApiSettings> _apiSettingsOptions;

    public SharepointGraphAgent(IOptions<ApiSettings> apiSettingsOptions)
    {
        _apiSettingsOptions = apiSettingsOptions;
    }

    private async Task<AuthTokenResponse> GetAuthTokenAsync()
    {
        var authUrl = "https://login.microsoftonline.com/305da883-09e0-43e2-aa5c-ad85d4d11078/oauth2/v2.0/token";
        var token = await Policy
            .Handle<FlurlHttpException>()
            .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
            .ExecuteAsync(() =>
                authUrl
                    .WithHeader("Accept", "application/json")
                    .WithTimeout(5)
                    .PostUrlEncodedAsync(new
                    {
                        grant_type = "client_credentials",
                        client_id = _apiSettingsOptions.Value.SharepointCredentials.ClientId,
                        client_secret = _apiSettingsOptions.Value.SharepointCredentials.ClientSecret,
                        scope = "https://graph.microsoft.com/.default"
                    })
                    .ReceiveJson<AuthTokenResponse>()
            );
        
        return token;
    }
    
    public async Task PostRequestAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> GetFileAttachmentAsync(string fileId)
    {
        var graphClient = new GraphServiceClient(new DelegateAuthenticationProvider(async request =>
        {
            var token = await GetAuthTokenAsync();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
        }));

        var listItemCollectionResponse = await graphClient
            .Sites["lucasas1.sharepoint.com,a7cce245-b8ac-4012-b04c-afc944f58718,aea09b6e-2511-4f3c-bcf6-d8b7a8253c16"]
            .Lists["336b8969-0054-4c49-ac33-c387269122d6"]
            .Items
            .Request()
            .Expand("fields, driveItem")
            .GetAsync();

        var files = new List<Stream>();
        
        foreach (var listItem in listItemCollectionResponse.CurrentPage)
        {
            var downloadedFile = await Policy
                .Handle<FlurlHttpException>()
                .WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(0.5))
                .ExecuteAsync(() =>
                    listItem.DriveItem.AdditionalData.First().Value.ToString()
                        .GetAsync()
                ).ReceiveStream();
            
            files.Add(downloadedFile);
        }

        return files.FirstOrDefault();
    }
}