using TravelPlanner.App.Models;

namespace TravelPlanner.App.Services
{
    public interface IApiService
    {
        // Trip methods
        Task<List<Trip>> GetTripsAsync();
        Task<Trip> GetTripAsync(int id);
        Task<Trip> AddTripAsync(Trip trip);
        Task UpdateTripAsync(int id, Trip trip);
        Task DeleteTripAsync(int id);

        // Stop methods
        Task<List<Stop>> GetStopsForTripAsync(int tripId);
        Task<Stop> AddStopAsync(Stop stop);
        Task UpdateStopAsync(int id, Stop stop);
        Task DeleteStopAsync(int id);
    }
}