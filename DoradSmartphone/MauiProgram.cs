using DoradSmartphone.Data;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using DoradSmartphone.Views;

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

        builder.Services.AddTransient<GeneralPage>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<UserPage>();        
        builder.Services.AddTransient<GlassPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<AvatarPage>();
        builder.Services.AddSingleton<WidgetPage>();
        builder.Services.AddSingleton<ManualPage>();
        builder.Services.AddSingleton<ChoicePage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<ExercisePage>();
        builder.Services.AddTransient<StartRunPage>();
        builder.Services.AddSingleton<AutomaticPage>();
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<CalibrationPage>();
        builder.Services.AddSingleton<DisplaySelectedItemsPage>();        

        builder.Services.AddSingleton<UserService>();
        builder.Services.AddSingleton<LoginService>();        
        builder.Services.AddSingleton<ExerciseService>();
        builder.Services.AddSingleton<DashboardService>();
        
        builder.Services.AddTransient<BluetoothService>();
        builder.Services.AddScoped<IBluetoothService, BluetoothService>();

        builder.Services.AddSingleton<UserViewModel>();
        builder.Services.AddTransient<GlassViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<AvatarViewModel>();
        builder.Services.AddTransient<WidgetViewModel>();
        builder.Services.AddTransient<ManualViewModel>();
        builder.Services.AddTransient<ChoiceViewModel>();
        builder.Services.AddSingleton<LoadingViewModel>();        
        builder.Services.AddTransient<ExerciseViewModel>();
        builder.Services.AddTransient<AutomaticViewModel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<CalibrationViewModel>();

        return builder.Build();
	}
}
