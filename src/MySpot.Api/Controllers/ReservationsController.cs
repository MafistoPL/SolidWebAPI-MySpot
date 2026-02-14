using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers;

[ApiController]
[Route(Path)]
public class ReservationsController(
    ICommandHandler<ReserveParkingSpotForVehicleCommand> reserveParkingSpotForVehicleCommandHandler,
    ICommandHandler<ChangeReservationLicensePlateCommand> changeReservationLicensePlateCommandHandler,
    ICommandHandler<DeleteReservationCommand> deleteReservationCommandHandler,
    ICommandHandler<ReserveParkingSpotForCleaningCommand> reserveParkingSpotForCleaningCommandHandler,
    IQueryHandler<GetReservation, ReservationDto?> getReservationQueryHandler
    ) : ControllerBase
{
    public const string Path = "reservations";

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto?>> GetById(Guid id)
    {
        var reservation = await getReservationQueryHandler.HandleAsync(new GetReservation { Id = id });
        if (reservation is null)
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

        return CreatedAtAction(nameof(GetById), new { id = command.ReservationId }, null);
    }

    [HttpPost("cleaning")]
    public async Task<ActionResult> Post([FromBody] ReserveParkingSpotForCleaningCommand command)
    {
        await reserveParkingSpotForCleaningCommandHandler.HandleAsync(command);
        
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
