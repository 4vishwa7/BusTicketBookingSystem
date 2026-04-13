using Microsoft.AspNetCore.SignalR;

namespace BusBookingSystem.API.API.Hub;

public class SeatHub : Microsoft.AspNetCore.SignalR.Hub
{
    public async Task SendSeatUpdate(int busId, int seatNumber)
    {
        await Clients.All.SendAsync("ReceiveSeatUpdate", busId, seatNumber);
    }
}