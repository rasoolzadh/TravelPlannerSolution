using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using TravelPlanner.App.Services;
using TravelPlanner.App.ViewModels;
using TravelPlanner.App.Views;
 

namespace TravelPlanner.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
  
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            string baseApiUrl = DeviceInfo.Platform == DevicePlatform.Android
                                ? "http://192.168.1.39:5153"
                                : "http://localhost:5153";

            builder.Services.AddSingleton<IApiService>(new ApiService(baseApiUrl));

            // Register ViewModels
            builder.Services.AddSingleton<TripsViewModel>();
            builder.Services.AddTransient<TripDetailViewModel>();
            builder.Services.AddTransient<AddEditTripViewModel>();
            builder.Services.AddTransient<AddEditStopViewModel>();
            builder.Services.AddTransient<MapViewModel>();
            builder.Services.AddTransient<CalendarViewModel>();

            // Register Views
            builder.Services.AddSingleton<TripsListPage>();
            builder.Services.AddTransient<TripDetailPage>();
            builder.Services.AddTransient<AddEditTripPage>();
            builder.Services.AddTransient<AddEditStopPage>();
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<CalendarPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}