using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.SharePoint.Client;
using PnP.Framework;
using SharepointApiIIntegration.Domain.Interfaces.Agents;
using SharepointApiIIntegration.Domain.Model.Settings;

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

    private async Task<ClientContext> GetClientContextAsync()
    {
        var clientContext = await AuthenticationManager.CreateWithCertificate(
                _apiSettingsOptions.Value.SharepointCredentials.ClientId,
                System.Security.Cryptography.X509Certificates.StoreName.My,
                System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser,
                _apiSettingsOptions.Value.SharepointCredentials.Thumbprint,
                _apiSettingsOptions.Value.SharepointCredentials.TenantId
            ).GetContextAsync(_apiSettingsOptions.Value.SharepointCredentials.SharepointSite);
        
        return clientContext;
    }

    public async Task PostRequestAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Stream> GetFileAttachmentAsync(string fileId)
    {
        var clientContext = await GetClientContextAsync();

        clientContext.Load(
            clientContext.Web.Lists
        );
        
        await clientContext.ExecuteQueryAsync();

        var listItems = clientContext.Web.Lists
            .FirstOrDefault(x => x.Id.ToString() == _apiSettingsOptions.Value.SharepointCredentials.ListId)?
            .GetItems(CamlQuery.CreateAllItemsQuery());
        
        clientContext.Load(listItems);
        await clientContext.ExecuteQueryAsync();

        var item = listItems?.Where(x => x.Id == int.Parse(fileId)).FirstOrDefault();
        
        clientContext.Load(item!.AttachmentFiles);
        await clientContext.ExecuteQueryAsync();
        
        var attachment = clientContext.Web.GetFileByServerRelativeUrl(item.AttachmentFiles.FirstOrDefault()!.ServerRelativeUrl);
        var fileStream = attachment.OpenBinaryStream();

        clientContext.Load(attachment);
        await clientContext.ExecuteQueryAsync();

        
        return fileStream.Value;
    }
}