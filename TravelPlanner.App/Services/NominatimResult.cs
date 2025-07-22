using System.Text.Json.Serialization;

namespace TravelPlanner.App.Models
{
    // This class helps deserialize the response from the Nominatim API
    public class NominatimResult
    {
        [JsonPropertyName("lat")]
        public string lat { get; set; } = string.Empty;

        [JsonPropertyName("lon")]
        public string lon { get; set; } = string.Empty;
    }
}