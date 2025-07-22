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
        // Generate the HTML and load it into the WebView
        var htmlSource = await _viewModel.GenerateMapHtmlAsync();
        mapWebView.Source = new HtmlWebViewSource { Html = htmlSource };
    }
}