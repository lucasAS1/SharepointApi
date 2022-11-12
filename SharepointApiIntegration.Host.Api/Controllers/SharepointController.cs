using Microsoft.AspNetCore.Mvc;
using SharepointApiIIntegration.Domain.Interfaces.Agents;

namespace SharepointApiIntegration.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SharepointController : ControllerBase
{
    private readonly ISharepointAgent _sharepointAgent;

    public SharepointController(ISharepointAgent sharepointAgent)
    {
        _sharepointAgent = sharepointAgent;
    }

    [HttpGet]
    [Route("GetSharepointSiteLists")]
    public async Task<IActionResult> GetSharepointSiteLists([FromQuery]string fileId)
    {
        var file = await _sharepointAgent.GetFileAttachmentAsync(fileId);

        return File(file, "application/octet-stream",fileDownloadName:"downloadedFile.pdf");
    }
}