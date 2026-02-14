using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers;

[ApiController]
[Route(Path)]
public class ParkingSpotsController(
    IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> getWeeklyParkingSpotsQueryHandler
    ) : ControllerBase
{
    public const string Path = "parking-spots";
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeeklyParkingSpotDto>>> Get([FromQuery] GetWeeklyParkingSpots query) 
        => Ok(await getWeeklyParkingSpotsQueryHandler.HandleAsync(query));
}
