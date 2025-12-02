using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Utils;

namespace Portfolio.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class PingController(IDateTimeProvider dateTimeProvider) : ControllerBase
{
    [HttpGet]
    public ActionResult<string> Ping()
    {
        return Ok(dateTimeProvider.Now);
    }
}