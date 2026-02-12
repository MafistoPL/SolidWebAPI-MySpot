using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController(IReservationsService reservationsService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> Get() => Ok(await reservationsService.GetAllWeeklyAsync());

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
        Guid? id = await reservationsService.ReserveForVehicleAsync(command with { ReservationId = Guid.NewGuid() });
        if (id == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { id = id }, null);
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
