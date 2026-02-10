using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repository;

public class InMemoryReservationRepository : IReservationRepository
{
    private static List<Reservation> _reservations = new();

    public Task<Reservation?> GetAsync(ReservationId id)
        => Task.FromResult(
            _reservations.SingleOrDefault(
                reservation => reservation.Id == id));
    
    public Task<IEnumerable<Reservation>> GetAllAsync()
        => Task.FromResult(_reservations.AsEnumerable());

    public Task AddAsync(Reservation reservation)
    {
        _reservations.Add(reservation);
        
        return Task.CompletedTask;        
    }

    public Task UpdateAsync(Reservation reservation)
    {
        return Task.CompletedTask;        
    }

    public Task RemoveAsync(Reservation reservation)
    {
        _reservations.Remove(reservation);
        
        return Task.CompletedTask;
    }
}
