using TravelPlanner.App.ViewModels;

namespace TravelPlanner.App.Views;

public partial class TripDetailPage : ContentPage
{
    private readonly TripDetailViewModel _viewModel;
    public TripDetailPage(TripDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadTripAsync();
    }
}