using ClientApp.Services;
using ClientApp.ViewModels;
using ClientApp.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Devices;
using System.Net.Http;

namespace ClientApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var apiBaseAddress = DeviceInfo.Platform == DevicePlatform.Android
                ? "http://10.0.2.2:5253/"
                : "http://localhost:5253/";

            builder.Services
                .AddHttpClient<ApiService>(client =>
                {
                    client.BaseAddress = new Uri(apiBaseAddress);
                    client.Timeout = TimeSpan.FromSeconds(10);
                })
                .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                {
                    ConnectTimeout = TimeSpan.FromSeconds(5)
                });

            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<ItemsPage>();
            builder.Services.AddTransient<ActionPage>();

            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<ItemsViewModel>();
            builder.Services.AddTransient<ActionViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
