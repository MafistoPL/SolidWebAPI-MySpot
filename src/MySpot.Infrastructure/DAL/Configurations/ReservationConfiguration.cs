using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Configurations;

internal sealed class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(reservation => reservation.Id);
        builder.Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ReservationId(x));
        builder.Property(x => x.ParkingSpotId)
            .HasConversion(x => x.Value, x => new ParkingSpotId(x));
        
        builder.Property(x => x.Date)
            .HasConversion(x => x.Value, x => new Date(x));
        
        builder.Property(x => x.Capacity)
            .HasConversion(
                x => (int)(ParkingSpotCapacityValue)x,
                x => new ParkingSpotCapacity((ParkingSpotCapacityValue)x));

        builder
            .HasDiscriminator<string>("Type")
            .HasValue<VehicleReservation>(nameof(VehicleReservation))
            .HasValue<CleaningReservation>(nameof(CleaningReservation));
    }
}
