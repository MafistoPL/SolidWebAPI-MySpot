using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route(Path)]
public class ReservationsController(IReservationsService reservationsService,
    ICommandHandler<ReserveParkingSpotForVehicleCommand> reserveParkingSpotForVehicleCommandHandler,
    ICommandHandler<ChangeReservationLicensePlateCommand> changeReservationLicensePlateCommandHandler,
    ICommandHandler<DeleteReservationCommand> deleteReservationCommandHandler,
    IQueryHandler<GetWeeklyParkingSpots, IEnumerable<WeeklyParkingSpotDto>> getWeeklyParkingSpotsQueryHandler
    ) : ControllerBase
{
    public const string Path = "reservations";
    
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
        await changeReservationLicensePlateCommandHandler.HandleAsync(command);
        
        return Ok();
    }

    [HttpDelete()]
    public async Task<ActionResult> Delete(DeleteReservationCommand command)
    {
        await deleteReservationCommandHandler.HandleAsync(command);

        return NoContent();
    }
}
