namespace BusBookingSystem.API.Models
{
    public class BusOperator
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Bus> Buses { get; set; } = new List<Bus>();
    }
}
