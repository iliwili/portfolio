using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Business.Auth.Services;

namespace Portfolio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlugController(IAuthService authService)  : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<string> GenerateSlug([FromQuery] string name)
    {
        var generatedSlug = authService.GenerateSlug(name);
        return Ok(generatedSlug);
    }
}