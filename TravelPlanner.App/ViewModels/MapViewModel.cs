using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TravelPlanner.App.Models;
using TravelPlanner.App.Services;
using TravelPlanner.App.Views;

namespace TravelPlanner.App.ViewModels
{
    public partial class TripsViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;
        public ObservableCollection<Trip> Trips { get; } = new();

        public TripsViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "My Trips ✈️";
        }

        [RelayCommand]
        public async Task LoadTripsAsync() // Changed from private to public
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Trips.Clear();
                var trips = await _apiService.GetTripsAsync();
                foreach (var trip in trips)
                {
                    Trips.Add(trip);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load trips: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToTripDetailsAsync(Trip trip)
        {
            if (trip == null) return;
            await Shell.Current.GoToAsync(nameof(TripDetailPage), true, new Dictionary<string, object>
            {
                { "TripId", trip.Id }
            });
        }

        [RelayCommand]
        async Task AddTripAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddEditTripPage), true);
        }

        [RelayCommand]
        async Task DeleteTripAsync(Trip trip)
        {
            if (trip == null) return;

            bool confirmed = await Shell.Current.DisplayAlert("Confirm Delete", $"Are you sure you want to delete '{trip.Title}'?", "Yes", "No");
            if (confirmed)
            {
                IsBusy = true;
                try
                {
                    await _apiService.DeleteTripAsync(trip.Id);
                    Trips.Remove(trip);
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", $"Failed to delete trip: {ex.Message}", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}