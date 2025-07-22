using TravelPlanner.App.ViewModels;

namespace TravelPlanner.App.Views;

public partial class TripsListPage : ContentPage
{
    private readonly TripsViewModel _viewModel;

    public TripsListPage(TripsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Using Task.Run to avoid blocking the UI thread
        Task.Run(() => _viewModel.LoadTripsCommand.Execute(null));
    }
}