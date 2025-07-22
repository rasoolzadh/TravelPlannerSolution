using System.Net.Http.Json;
using System.Text.Json;
using TravelPlanner.App.Models;

namespace TravelPlanner.App.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions;

        public ApiService(string baseUri)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUri) };
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // This is the helper method that reads the detailed error
        private async Task HandleErrors(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // This will throw an exception with the detailed error from the API
                throw new HttpRequestException(errorContent, null, response.StatusCode);
            }
        }

        public async Task<List<Trip>> GetTripsAsync()
        {
            var response = await _httpClient.GetAsync("api/trips");
            await HandleErrors(response);
            return await response.Content.ReadFromJsonAsync<List<Trip>>(_serializerOptions) ?? new();
        }

        public async Task<Trip> GetTripAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/trips/{id}");
            await HandleErrors(response);
            return await response.Content.ReadFromJsonAsync<Trip>(_serializerOptions) ?? new();
        }

        public async Task<Trip> AddTripAsync(Trip trip)
        {
            var response = await _httpClient.PostAsJsonAsync("api/trips", trip);
            await HandleErrors(response);
            return await response.Content.ReadFromJsonAsync<Trip>() ?? new();
        }

        public async Task UpdateTripAsync(int id, Trip trip)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/trips/{id}", trip);
            await HandleErrors(response);
        }

        public async Task DeleteTripAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/trips/{id}");
            await HandleErrors(response);
        }

        public async Task<List<Stop>> GetStopsForTripAsync(int tripId)
        {
            var response = await _httpClient.GetAsync($"api/stops/ByTrip/{tripId}");
            await HandleErrors(response);
            return await response.Content.ReadFromJsonAsync<List<Stop>>(_serializerOptions) ?? new();
        }

        public async Task<Stop> AddStopAsync(Stop stop)
        {
            var response = await _httpClient.PostAsJsonAsync("api/stops", stop);
            await HandleErrors(response); // Now uses the new error handler
            return await response.Content.ReadFromJsonAsync<Stop>() ?? new();
        }

        public async Task UpdateStopAsync(int id, Stop stop)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/stops/{id}", stop);
            await HandleErrors(response);
        }

        public async Task DeleteStopAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/stops/{id}");
            await HandleErrors(response);
        }
    }
}