using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TravelPlanner.App.Models;
using TravelPlanner.App.Services;

namespace TravelPlanner.App.ViewModels
{
    [QueryProperty(nameof(TripToEdit), "TripToEdit")]
    public partial class AddEditTripViewModel : BaseViewModel
    {
        private readonly IApiService _apiService;

        [ObservableProperty]
        Trip trip = new();

        [ObservableProperty]
        Trip tripToEdit = default!; // Fixed warning

        private bool IsEditMode => tripToEdit != null;

        public AddEditTripViewModel(IApiService apiService)
        {
            _apiService = apiService;
            Title = "Add New Trip";
        }

        partial void OnTripToEditChanged(Trip value)
        {
            if (value != null)
            {
                Trip = new Trip
                {
                    Id = value.Id,
                    Title = value.Title,
                    Description = value.Description,
                    StartDate = value.StartDate,
                    EndDate = value.EndDate,
                    Budget = value.Budget,
                };
                Title = "Edit Trip";
            }
        }

        [RelayCommand]
        async Task SaveTripAsync()
        {
            if (string.IsNullOrWhiteSpace(Trip.Title) || Trip.EndDate < Trip.StartDate)
            {
                await Shell.Current.DisplayAlert("Validation Error", "Please provide a valid title and ensure the end date is after the start date.", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                if (IsEditMode)
                {
                    await _apiService.UpdateTripAsync(Trip.Id, Trip);
                }
                else
                {
                    await _apiService.AddTripAsync(Trip);
                }
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to save trip: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}