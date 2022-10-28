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
    public async Task<IActionResult> GetSharepointSiteLists()
    {
        var lists = await _sharepointAgent.GetFileAttachmentAsync("1");

        return Ok(new ByteArrayContent(lists));
    }
}