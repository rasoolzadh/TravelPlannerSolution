using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TravelPlanner.App.Models;

namespace TravelPlanner.App.ViewModels
{
    [QueryProperty(nameof(Trip), "Trip")]
    public partial class MapViewModel : BaseViewModel
    {
        [ObservableProperty]
        Trip trip = default!;

        public MapViewModel()
        {
            Title = "Trip Map";
        }

        // This is the geocoding method you created
        private async Task<(double? Latitude, double? Longitude)> TryGeocodeWithNominatim(string placeName)
        {
            try
            {
                // Use a static HttpClient to be more efficient
                using var httpClient = new HttpClient();
                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(placeName)}&format=json&limit=1";

                // Nominatim requires a unique User-Agent header
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("TravelPlannerApp/1.0 (your-email@example.com)");

                var response = await httpClient.GetStringAsync(url);
                var results = JsonSerializer.Deserialize<List<NominatimResult>>(response);

                var result = results?.FirstOrDefault();
                if (result != null && !string.IsNullOrEmpty(result.lat) && !string.IsNullOrEmpty(result.lon))
                {
                    return (
                        double.Parse(result.lat, CultureInfo.InvariantCulture),
                        double.Parse(result.lon, CultureInfo.InvariantCulture)
                    );
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Nominatim geocoding failed for '{placeName}': {ex.Message}");
            }

            return (null, null);
        }

        // This method is now updated to generate a route with all stops
        public async Task<string> GetMapUrlAsync()
        {
            if (trip == null || !trip.Stops.Any())
            {
                await Shell.Current.DisplayAlert("No Stops", "This trip has no stops to display.", "OK");
                return "about:blank";
            }

            var coordinates = new List<string>();

            // Loop through all stops, not just the first one
            foreach (var stop in trip.Stops.OrderBy(s => s.ArrivalDate))
            {
                var (lat, lon) = await TryGeocodeWithNominatim(stop.Location);
                if (lat.HasValue && lon.HasValue)
                {
                    // Format as lat,lon
                    var latStr = lat.Value.ToString(CultureInfo.InvariantCulture);
                    var lonStr = lon.Value.ToString(CultureInfo.InvariantCulture);
                    coordinates.Add($"{latStr},{lonStr}");
                }
            }

            if (coordinates.Count < 2)
            {
                await Shell.Current.DisplayAlert("Not Enough Locations", "Could not find coordinates for enough stops to create a route.", "OK");
                return "about:blank";
            }

            // Join the coordinates with a semicolon for the URL
            var routeCoordinates = string.Join(";", coordinates);

            // Construct the OpenStreetMap directions URL
            return $"https://www.openstreetmap.org/directions?engine=fossgis_osrm_car&route={routeCoordinates}";
        }
    }
}