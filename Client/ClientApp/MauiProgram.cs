using Microsoft.Extensions.Logging;

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

            builder.Services.AddSingleton<ViewModels.LoginViewModel>();
            builder.Services.AddSingleton<ViewModels.ItemsViewModel>();
            builder.Services.AddTransient<ViewModels.ActionViewModel>();

            builder.Services.AddSingleton<Views.LoginPage>();
            builder.Services.AddSingleton<Views.ItemsPage>();
            builder.Services.AddTransient<Views.ActionPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
