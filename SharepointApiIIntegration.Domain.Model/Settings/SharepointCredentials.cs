namespace SharepointApiIIntegration.Domain.Model.Settings;

public class SharepointCredentials
{
    public string ListId { get; set; }
    public string Thumbprint { get; set; }
    public string TenantId { get; set; }
    public string SharepointSite { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}