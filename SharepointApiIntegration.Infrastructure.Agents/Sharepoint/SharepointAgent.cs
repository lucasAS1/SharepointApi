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

        await LoadClientContext(clientContext, clientContext.Web.Lists);

        var listItems = clientContext.Web.Lists
            .FirstOrDefault(x => x.Id.ToString() == _apiSettingsOptions.Value.SharepointCredentials.ListId)?
            .GetItems(CamlQuery.CreateAllItemsQuery());

        await LoadClientContext(clientContext, listItems!);

        var item = listItems?.Where(x => x.Id == int.Parse(fileId)).FirstOrDefault();
        
        await LoadClientContext(clientContext, item!.AttachmentFiles);
        
        var attachment = clientContext.Web.GetFileByServerRelativeUrl(item.AttachmentFiles.FirstOrDefault()!.ServerRelativeUrl);
        var fileStream = attachment.OpenBinaryStream();
        
        await LoadClientContext(clientContext, attachment);

        return fileStream.Value;
    }

    private async Task LoadClientContext(ClientContext clientContext, ClientObject itemToBeLoaded)
    {
        clientContext.Load(itemToBeLoaded);
        await clientContext.ExecuteQueryAsync();
    }
}