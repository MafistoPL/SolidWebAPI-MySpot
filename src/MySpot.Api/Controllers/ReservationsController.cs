using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Commands;
using MySpot.Api.DTO;
using MySpot.Api.Entities;
using MySpot.Api.services;
using MySpot.Api.ValueObjects;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private static readonly Clock Clock = new();
    private static readonly List<WeeklyParkingSpot> WeeklyParkingSpots = [
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000001"), 
            new Week(Clock.Current()), "P1"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000002"),
            new Week(Clock.Current()), "P2"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000003"),
            new Week(Clock.Current()), "P3"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000004"),
            new Week(Clock.Current()), "P4"),
        new WeeklyParkingSpot(Guid.Parse("00000000-0000-0000-0000-000000000005"), 
            new Week(Clock.Current()), "P5")
    ];
    
    private static readonly ReservationsService ReservationsService 
        = new ReservationsService(WeeklyParkingSpots, Clock);
    
    [HttpGet]
    public ActionResult<IEnumerable<ReservationDto>> Get() => Ok(ReservationsService.GetAllWeekly());

    [HttpGet("{id:guid}")]
    public ActionResult<ReservationDto?> Get(Guid id)
    {
        ReservationDto? reservation = ReservationsService.Get(id);

        if (reservation == null)
        {
            return NotFound();
        }
        
        return Ok(reservation);
    }
    
    [HttpPost]
    public ActionResult Post([FromBody] CreateReservationCommand command)
    {
        Guid? id = ReservationsService.Create(command with { ReservationId = Guid.NewGuid() });
        if (id == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new { id = id }, null);
    }

    [HttpPut()]
    public ActionResult Put(ChangeReservationLicensePlateCommand command)
    {
        if (ReservationsService.Update(command) == false)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete()]
    public ActionResult Delete(DeleteReservationCommand command)
    {
        if (ReservationsService.Delete(command) == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}
