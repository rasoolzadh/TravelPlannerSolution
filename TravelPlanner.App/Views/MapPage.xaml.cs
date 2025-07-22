using TravelPlanner.App.ViewModels;

namespace TravelPlanner.App.Views;

public partial class MapPage : ContentPage
{
    private readonly MapViewModel _viewModel;

    public MapPage(MapViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var url = await _viewModel.GetMapUrlAsync();
        mapWebView.Source = new UrlWebViewSource { Url = url };
    }
}