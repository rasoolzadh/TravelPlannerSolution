using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TravelPlanner.Api.Models
{
    public class Trip
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public ICollection<Stop> Stops { get; set; } = new List<Stop>();
    }
}