using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BusBookingSystem.API.Domain.Entities;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.UserId);

        builder.Property(b => b.PassengerName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Phone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(b => b.TotalPrice)
            .HasColumnType("decimal(10,2)");

        builder.Property(b => b.Status)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();
    }
}