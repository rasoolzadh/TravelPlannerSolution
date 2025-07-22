using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using TravelPlanner.App.Models;

namespace TravelPlanner.App.ViewModels
{
    [QueryProperty(nameof(Trip), "Trip")]
    public partial class CalendarViewModel : BaseViewModel
    {
        [ObservableProperty]
        Trip trip = default!;

        // This property is bound to the DatePicker.
        // When it changes, the OnSelectedDateChanged method is automatically called.
        [ObservableProperty]
        DateTime selectedDate;

        public ObservableCollection<Stop> StopsForSelectedDate { get; } = new();

        public CalendarViewModel()
        {
            Title = "Trip Calendar";
        }

        // This method runs when the 'Trip' property is set by navigation
        partial void OnTripChanged(Trip value)
        {
            if (value != null)
            {
                // Set the initial date for the picker
                SelectedDate = value.StartDate;
            }
        }

        // This method runs whenever the 'SelectedDate' property changes
        partial void OnSelectedDateChanged(DateTime value)
        {
            FilterStopsForDate(value);
        }

        private void FilterStopsForDate(DateTime date)
        {
            if (trip == null) return;

            StopsForSelectedDate.Clear();
            var stopsOnDate = trip.Stops
                .Where(s => date.Date >= s.ArrivalDate.Date && date.Date <= s.DepartureDate.Date)
                .OrderBy(s => s.ArrivalDate);

            foreach (var stop in stopsOnDate)
            {
                StopsForSelectedDate.Add(stop);
            }
        }
    }
}