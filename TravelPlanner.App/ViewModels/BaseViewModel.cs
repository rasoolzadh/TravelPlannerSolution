using CommunityToolkit.Mvvm.ComponentModel;

namespace TravelPlanner.App.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;

        [ObservableProperty]
        string title = string.Empty; // Fixed warning

        public bool IsNotBusy => !IsBusy;
    }
}