using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySpot.Infrastructure;
using MySpot.Infrastructure.DAL;

namespace MySpot.Api.Controllers;

[Route("")]
public class HomeController(IOptionsSnapshot<AppOptions> appOptions)
{
    [HttpGet]
    public ActionResult<string> Get() => appOptions.Value.Name;
}