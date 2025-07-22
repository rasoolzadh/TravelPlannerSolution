namespace TravelPlanner.App.Models
{
    public class Stop
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime ArrivalDate { get; set; } = DateTime.Now;
        public DateTime DepartureDate { get; set; } = DateTime.Now;
        public decimal EstimatedCost { get; set; }
        public string Notes { get; set; } = string.Empty;
        public int TripId { get; set; }
    }
}