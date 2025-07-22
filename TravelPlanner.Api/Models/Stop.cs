using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.Api.Models
{
    public class Stop
    {
        public int Id { get; set; }
        [Required]
        public string Location { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public decimal EstimatedCost { get; set; }
        public string Notes { get; set; }

        // Foreign Key
        public int TripId { get; set; }
        [ForeignKey("TripId")]
        public Trip? Trip { get; set; }
    }
}