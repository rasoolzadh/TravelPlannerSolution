using System.Collections.ObjectModel;

namespace TravelPlanner.App.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);
        public decimal Budget { get; set; }
        public ObservableCollection<Stop> Stops { get; set; } = new();
    }
}