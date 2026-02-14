using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Core.Repositories;

namespace MySpot.Application.Commands.Handlers;

public class DeleteReservationCommandHandler(IReservationRepository reservationRepository) 
    : ICommandHandler<DeleteReservationCommand>
{
    public async Task HandleAsync(DeleteReservationCommand command)
    {
        var existingReservation = await reservationRepository.GetAsync(command.ReservationId);
        if (existingReservation == null)
        {
            throw new ReservationNotFoundException(command.ReservationId);
        }
        
        await reservationRepository.RemoveAsync(existingReservation);
    }
}