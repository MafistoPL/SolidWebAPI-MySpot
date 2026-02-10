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
    
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateReservationCommand command)
    {
        Guid? id = await reservationsService.CreateAsync(command with { ReservationId = Guid.NewGuid() });
        if (id == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { id = id }, null);
    }

    [HttpPut()]
    public async Task<ActionResult> Put(ChangeReservationLicensePlateCommand command)
    {
        if (!await reservationsService.UpdateAsync(command))
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
