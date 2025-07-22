using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelPlanner.App.Models;
using TravelPlanner.App.Services;

namespace TravelPlanner.App.ViewModels
{
    [QueryProperty(nameof(TripId), "TripId")]
    [QueryProperty(nameof(StopToEdit), "StopToEdit")]
    public partial class AddEditStopViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        Stop stop = new();

        [ObservableProperty]
        Stop stopToEdit = default!;

        [ObservableProperty]
        int tripId;

        private bool IsEditMode => stopToEdit != null;

        public AddEditStopViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Add New Stop";
        }

        partial void OnStopToEditChanged(Stop value)
        {
            if (value != null)
            {
                Stop = new Stop
                {
                    Id = value.Id,
                    TripId = value.TripId,
                    Location = value.Location,
                    ArrivalDate = value.ArrivalDate,
                    DepartureDate = value.DepartureDate,
                    EstimatedCost = value.EstimatedCost,
                    Notes = value.Notes
                };
                Title = "Edit Stop";
            }
        }

        partial void OnTripIdChanged(int value)
        {
            if (!IsEditMode)
            {
                Stop.TripId = value;
            }
        }

        [RelayCommand]
      
        async Task SaveStopAsync()
        {
            if (string.IsNullOrWhiteSpace(Stop.Location) || Stop.DepartureDate < Stop.ArrivalDate)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please provide a valid location and ensure the departure date is after the arrival date.", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                if (IsEditMode)
                {
                    await _apiService.UpdateStopAsync(Stop.Id, Stop);
                }
                else
                {
                    Stop.TripId = this.TripId;
                    await _apiService.AddStopAsync(Stop);
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (HttpRequestException httpEx) // This is the new, more specific catch block
            {
                // This will display the DETAILED error message from the API
                await Shell.Current.DisplayAlert("API Error", httpEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}