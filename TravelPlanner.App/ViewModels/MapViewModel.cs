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

        private async Task<(double? Latitude, double? Longitude)> TryGeocodeWithNominatim(string placeName)
        {
            try
            {
                using var httpClient = new HttpClient();
                var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(placeName)}&format=json&limit=1";
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("TravelPlannerApp/1.0");

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

        public async Task<string> GenerateMapHtmlAsync()
        {
            IsBusy = true;
            try
            {
                if (trip == null || !trip.Stops.Any())
                {
                    return GetErrorHtml("This trip has no stops to display.");
                }

                var coordinates = new StringBuilder();
                var locationsFound = 0;

                foreach (var stop in trip.Stops.OrderBy(s => s.ArrivalDate))
                {
                    var (lat, lon) = await TryGeocodeWithNominatim(stop.Location);
                    if (lat.HasValue && lon.HasValue)
                    {
                        var latStr = lat.Value.ToString(CultureInfo.InvariantCulture);
                        var lonStr = lon.Value.ToString(CultureInfo.InvariantCulture);
                        var title = stop.Location.Replace("'", "\\'");

                        coordinates.Append($"[{latStr}, {lonStr}, '{title}'],");
                        locationsFound++;
                    }
                }

                if (locationsFound == 0)
                {
                    return GetErrorHtml("Could not find coordinates for any stops in this trip.");
                }

                return GetMapHtml(coordinates.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }

        private string GetMapHtml(string coordinatesJsArray)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <title>Trip Stops</title>
    <meta charset=""utf-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link rel=""stylesheet"" href=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.css"" />
    <script src=""https://unpkg.com/leaflet@1.9.4/dist/leaflet.js""></script>
    <style>
        html, body, #map {{ height: 100%; width: 100%; margin: 0; padding: 0; }}
    </style>
</head>
<body>
    <div id=""map""></div>
    <script>
        var map = L.map('map');
        L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
            attribution: '&copy; <a href=""https://www.openstreetmap.org/copyright"">OpenStreetMap</a> contributors'
        }}).addTo(map);

        var points = [{coordinatesJsArray}];
        var markers = [];

        for (var i = 0; i < points.length; i++) {{
            var marker = L.marker([points[i][0], points[i][1]]).addTo(map)
                .bindPopup(points[i][2]);
            markers.push(marker);
        }}

        if (markers.length > 0) {{
            var group = L.featureGroup(markers);
            map.fitBounds(group.getBounds().pad(0.5));
        }}
    </script>
</body>
</html>";
        }

        private string GetErrorHtml(string message)
        {
            return $@"
<!DOCTYPE html>
<html>
<head><title>Error</title></head>
<body style='font-family: sans-serif; text-align: center; padding-top: 20px;'>
    <h2>Map Not Available</h2>
    <p>{message}</p>
</body>
</html>";
        }
    }
}