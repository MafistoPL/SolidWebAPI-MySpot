using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

public class InMemoryReservationRepository : IReservationRepository
{
    private static List<Reservation> _reservations = new();

    public Reservation? Get(ReservationId id)
        => _reservations.SingleOrDefault(reservation => reservation.Id == id);

    public IEnumerable<Reservation> GetAll()
        => _reservations;

    public void Add(Reservation reservation)
        => _reservations.Add(reservation);

    public void Update(Reservation reservation)
    {
    }

    public void Remove(Reservation reservation)
        => _reservations.Remove(reservation);
}
