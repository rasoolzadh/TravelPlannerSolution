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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // This is a much safer way to load data on page appearance
        await _viewModel.LoadTripsAsync();
    }
}