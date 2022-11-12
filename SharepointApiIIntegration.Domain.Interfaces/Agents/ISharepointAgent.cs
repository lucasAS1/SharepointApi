﻿
using Flurl.Http;

namespace SharepointApiIIntegration.Domain.Interfaces.Agents;

public interface ISharepointAgent
{
    public Task PostRequestAsync();
    public Task<Stream> GetFileAttachmentAsync(string fileId);
}