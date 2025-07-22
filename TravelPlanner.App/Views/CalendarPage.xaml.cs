using TravelPlanner.App.ViewModels;

namespace TravelPlanner.App.Views;

public partial class CalendarPage : ContentPage
{
    public CalendarPage(CalendarViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}