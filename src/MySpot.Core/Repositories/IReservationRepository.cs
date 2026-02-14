using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositories;

public interface IReservationRepository
{
    public Task<Reservation?> GetAsync(ReservationId id);
    public Task<IEnumerable<Reservation>> GetAllAsync();
    Task AddAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task RemoveAsync(Reservation reservation);
    Task RemoveAsync(IEnumerable<Reservation> reservations);
    
}
