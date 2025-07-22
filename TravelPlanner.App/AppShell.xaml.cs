using TravelPlanner.App.Views;

namespace TravelPlanner.App;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Register Routes for Navigation
        Routing.RegisterRoute(nameof(TripDetailPage), typeof(TripDetailPage));
        Routing.RegisterRoute(nameof(AddEditTripPage), typeof(AddEditTripPage));
        Routing.RegisterRoute(nameof(AddEditStopPage), typeof(AddEditStopPage));
        Routing.RegisterRoute(nameof(MapPage), typeof(MapPage));
        Routing.RegisterRoute(nameof(CalendarPage), typeof(CalendarPage));
    }
}