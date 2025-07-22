using TravelPlanner.App.ViewModels;

namespace TravelPlanner.App.Views;

public partial class AddEditStopPage : ContentPage
{
    public AddEditStopPage(AddEditStopViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}