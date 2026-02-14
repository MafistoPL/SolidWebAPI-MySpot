using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController(IReservationsService reservationsService,
    ICommandHandler<ReserveParkingSpotForVehicleCommand> reserveParkingSpotForVehicleCommandHandler,
    IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> getWeeklyParkingSpotsQueryHandler
    ) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> Get([FromQuery] GetWeeklyParkingSpots query) 
        => Ok(await getWeeklyParkingSpotsQueryHandler.HandleAsync(query));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto?>> Get(Guid id)
    {
        ReservationDto? reservation = await reservationsService.GetAsync(id);

        if (reservation == null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }
    
    [HttpPost("vehicle")]
    public async Task<ActionResult> Post([FromBody] ReserveParkingSpotForVehicleCommand command)
    {
        command = command with { ReservationId = Guid.NewGuid() };
        await reserveParkingSpotForVehicleCommandHandler.HandleAsync(command);

        return CreatedAtAction(nameof(Get), new { id = command.ReservationId }, null);
    }

    [HttpPost("cleaning")]
    public async Task<ActionResult> Post([FromBody] ReserveParkingSpotForCleaningCommand command)
    {
        await reservationsService.ReserveForCleaningAsync(command);
        
        return Ok();
    }

    [HttpPut()]
    public async Task<ActionResult> Put(ChangeReservationLicensePlateCommand command)
    {
        if (!await reservationsService.ChangeReservationLicensePlateAsync(command))
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete()]
    public async Task<ActionResult> Delete(DeleteReservationCommand command)
    {
        if (!await reservationsService.DeleteAsync(command))
        {
            return NotFound();
        }

        return NoContent();
    }
}
