using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController(IReservationsService reservationsService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ReservationDto>> Get() => Ok(reservationsService.GetAllWeekly());

    [HttpGet("{id:guid}")]
    public ActionResult<ReservationDto?> Get(Guid id)
    {
        ReservationDto? reservation = reservationsService.Get(id);

        if (reservation == null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }
    
    [HttpPost]
    public ActionResult Post([FromBody] CreateReservationCommand command)
    {
        Guid? id = reservationsService.Create(command with { ReservationId = Guid.NewGuid() });
        if (id == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { id = id }, null);
    }

    [HttpPut()]
    public ActionResult Put(ChangeReservationLicensePlateCommand command)
    {
        if (reservationsService.Update(command) == false)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete()]
    public ActionResult Delete(DeleteReservationCommand command)
    {
        if (reservationsService.Delete(command) == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}
