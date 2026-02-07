using MySpot.Api.Commands;
using MySpot.Api.services;
using Shouldly;

namespace MySpot.Tests.Unit.Services;

public class ReservationsServiceTests
{
    [Fact]
    public void Create_WithCorrectDate_ShouldSucceed()
    {
        var command = new CreateReservationCommand(
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Guid.NewGuid(),
            DateTime.UtcNow,
            "John Doe",
            "ABC-123"
            );
        
        var reservationId = _reservationsService.Create(command);

        reservationId.ShouldNotBeNull();
    }
    
    #region Arrange
    
    private readonly ReservationsService _reservationsService;

    public ReservationsServiceTests()
    {
        _reservationsService = new ReservationsService();
    }
    
    #endregion
}