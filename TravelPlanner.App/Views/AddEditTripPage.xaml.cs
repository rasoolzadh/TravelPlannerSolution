using TravelPlanner.App.ViewModels;

namespace TravelPlanner.App.Views;

public partial class AddEditTripPage : ContentPage
{
    public AddEditTripPage(AddEditTripViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}