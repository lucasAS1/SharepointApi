
using SharepointApiIIntegration.Domain.Model.Responses;

namespace SharepointApiIIntegration.Domain.Interfaces.Agents;

public interface ISharepointAgent
{
    public Task<Stream> GetFileAttachmentAsync(string fileId);
    public Task<List<GetListItemsResponse>> GetListItemsResponse();
}