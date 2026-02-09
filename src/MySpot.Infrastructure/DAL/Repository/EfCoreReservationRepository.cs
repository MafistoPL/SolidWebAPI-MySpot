using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

public class EfCoreReservationRepository(MySpotDbContext context)
    : IReservationRepository
{
    public Reservation? Get(ReservationId id)
        => context.Reservations.SingleOrDefault(reservation => reservation.Id == id);

    public IEnumerable<Reservation> GetAll()
        => context.Reservations.ToList();

    public void Add(Reservation reservation)
    {
        context.Reservations.Add(reservation);
        context.SaveChanges();
    }

    public void Update(Reservation reservation)
    {
        context.Reservations.Update(reservation);
        context.SaveChanges();
    }

    public void Remove(Reservation reservation)
    {
        context.Reservations.Remove(reservation);
        context.SaveChanges();
    }
}
