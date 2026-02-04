using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
using MySpot.Api.services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private static readonly ReservationsService ReservationsService = new ReservationsService();
    
    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(ReservationsService.Get());

    [HttpGet("{id:int}")]
    public ActionResult<Reservation?> Get(int id)
    {
        Reservation? reservation = ReservationsService.Get(id);

        if (reservation == null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }
    
    [HttpPost]
    public ActionResult Post([FromBody] Reservation reservation)
    {
        int? id = ReservationsService.Create(reservation);
        if (id == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { id = id }, reservation);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Reservation reservation)
    {
        if (ReservationsService.Put(id, reservation) == false)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        if (ReservationsService.Delete(id) == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}
