using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel.DataTransfer;
using System.Collections.ObjectModel;
using System.Text;
using TravelPlanner.App.Models;
using TravelPlanner.App.Services;
using TravelPlanner.App.Views;

namespace TravelPlanner.App.ViewModels
{
    [QueryProperty(nameof(TripId), "TripId")]
    public partial class TripDetailViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        int tripId;

        [ObservableProperty]
        Trip currentTrip = default!;

        public ObservableCollection<Stop> Stops { get; } = new();

        [ObservableProperty]
        decimal totalCost;

        [ObservableProperty]
        bool isOverBudget;

        public TripDetailViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Trip Details";
        }

        public async Task LoadTripAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                CurrentTrip = await _apiService.GetTripAsync(TripId);
                Stops.Clear();
                if (CurrentTrip?.Stops != null)
                {
                    foreach (var stop in CurrentTrip.Stops.OrderBy(s => s.ArrivalDate))
                    {
                        Stops.Add(stop);
                    }
                }
                Title = CurrentTrip?.Title ?? "Trip Details";
                CalculateCost();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to load trip details: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void CalculateCost()
        {
            if (CurrentTrip == null) return;
            TotalCost = Stops.Sum(s => s.EstimatedCost);
            IsOverBudget = CurrentTrip.Budget > 0 && TotalCost > CurrentTrip.Budget;
        }

        [RelayCommand]
        async Task EditTripAsync()
        {
            if (CurrentTrip == null) return;
            await Shell.Current.GoToAsync(nameof(AddEditTripPage), true, new Dictionary<string, object>
            {
                { "TripToEdit", CurrentTrip }
            });
        }

        [RelayCommand]
        async Task AddStopAsync()
        {
            if (CurrentTrip == null) return;
            await Shell.Current.GoToAsync(nameof(AddEditStopPage), true, new Dictionary<string, object>
            {
                { "TripId", TripId }
            });
        }

        [RelayCommand]
        async Task EditStopAsync(Stop stop)
        {
            if (stop == null) return;
            await Shell.Current.GoToAsync(nameof(AddEditStopPage), true, new Dictionary<string, object>
            {
                { "StopToEdit", stop }
            });
        }

        [RelayCommand]
        async Task DeleteStopAsync(Stop stop)
        {
            if (stop == null) return;

            bool confirmed = await Shell.Current.DisplayAlert("Confirm Delete", $"Delete stop at '{stop.Location}'?", "Yes", "No");
            if (confirmed)
            {
                IsBusy = true;
                try
                {
                    await _apiService.DeleteStopAsync(stop.Id);
                    Stops.Remove(stop);
                    CalculateCost();
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", $"Failed to delete stop: {ex.Message}", "OK");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        // ✅ This is the missing command
        [RelayCommand]
        async Task ViewOnMapAsync()
        {
            if (CurrentTrip == null) return;
            await Shell.Current.GoToAsync(nameof(MapPage), true, new Dictionary<string, object>
            {
                { "Trip", CurrentTrip }
            });
        }

        [RelayCommand]
        async Task ViewCalendarAsync()
        {
            if (CurrentTrip == null) return;
            await Shell.Current.GoToAsync(nameof(CalendarPage), true, new Dictionary<string, object>
            {
                { "Trip", CurrentTrip }
            });
        }

        [RelayCommand]
        async Task ShareTripAsync()
        {
            if (CurrentTrip == null) return;

            var sb = new StringBuilder();
            sb.AppendLine($"✈️ Trip Plan: {CurrentTrip.Title}");
            sb.AppendLine($"🗓️ Dates: {CurrentTrip.StartDate:d} to {CurrentTrip.EndDate:d}");
            sb.AppendLine($"💰 Budget: {CurrentTrip.Budget:C}");
            sb.AppendLine($"💵 Total Cost: {TotalCost:C}");
            sb.AppendLine();
            sb.AppendLine("--- STOPS ---");

            foreach (var stop in Stops)
            {
                sb.AppendLine($"- {stop.Location} ({stop.ArrivalDate:d} - {stop.DepartureDate:d})");
                sb.AppendLine($"  Cost: {stop.EstimatedCost:C}");
                if (!string.IsNullOrWhiteSpace(stop.Notes))
                {
                    sb.AppendLine($"  Notes: {stop.Notes}");
                }
                sb.AppendLine();
            }

            await Share.RequestAsync(new ShareTextRequest
            {
                Text = sb.ToString(),
                Title = "Share Trip Plan"
            });
        }
    }
}