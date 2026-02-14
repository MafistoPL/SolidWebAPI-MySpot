using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Configurations;

internal sealed class WeeklyParkingSpotConfiguration : IEntityTypeConfiguration<WeeklyParkingSpot>
{
    public void Configure(EntityTypeBuilder<WeeklyParkingSpot> builder)
    {
        builder.HasKey(spot => spot.Id);
        builder.Property(spot => spot.Id)
            .HasConversion(x => x.Value, x => new ParkingSpotId(x));
        builder.Property(spot => spot.Name)
            .HasConversion(x => x.Value, x => new ParkingSpotName(x));
        builder.Property(spot => spot.Week)
            .HasConversion(x => x.To.Value, x => new Week(x));
        builder.Property(spot => spot.Capacity)
            .HasConversion(
                x => (int)(ParkingSpotCapacityValue)x,
                x => new ParkingSpotCapacity((ParkingSpotCapacityValue)x));
    }
}
