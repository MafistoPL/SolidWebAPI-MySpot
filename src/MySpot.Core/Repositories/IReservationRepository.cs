using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositories;

public interface IReservationRepository
{
    public Reservation? Get(ReservationId id);
    public IEnumerable<Reservation> GetAll();
    void Add(Reservation reservation);
    void Update(Reservation reservation);
    void Remove(Reservation reservation);
}
