using DoradSmartphone.Data;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using DoradSmartphone.Views;
using ToastProject;
using ToastProject.Platforms;

namespace DoradSmartphone;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()       
            .UseMauiMaps()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<DatabaseConn>();
        builder.Services.AddScoped<IRepository, DatabaseConn>();

        builder.Services.AddSingleton<IMap>(Map.Default);
        builder.Services.AddSingleton<IToast>((e) => new Toaster());
        builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);        

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddSingleton<UserPage>();
        builder.Services.AddSingleton<GlassPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<WidgetPage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<ExercisePage>();
        builder.Services.AddSingleton<DashboardPage>();        
        builder.Services.AddSingleton<ExerciseDetailsPage>();

        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<LoginService>();        
        builder.Services.AddSingleton<ExerciseService>();
        builder.Services.AddSingleton<DashboardService>();
        builder.Services.AddSingleton<BluetoothService>();
        builder.Services.AddSingleton<BluetoothLEService>();

        builder.Services.AddSingleton<UserViewModel>();
        builder.Services.AddSingleton<GlassViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<WidgetViewModel>();
        builder.Services.AddSingleton<LoadingViewModel>();
        builder.Services.AddTransient<ExerciseViewModel>();
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddTransient<ExerciseDetailsViewModel>();

        return builder.Build();
	}
}
