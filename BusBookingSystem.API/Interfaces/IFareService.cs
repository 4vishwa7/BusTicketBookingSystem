namespace BusBookingSystem.API.Interfaces
{
    public interface IFareService
    {
        Task<decimal> CalculateFare(Guid tripId, List<Guid> seatIds);
    }
}
