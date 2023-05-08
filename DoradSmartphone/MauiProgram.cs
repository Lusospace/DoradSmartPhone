using DoradSmartphone.Data;
using DoradSmartphone.Services;
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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<DatabaseConn>();

        builder.Services.AddSingleton<MainPage>();        
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<ExercisePage>();
        builder.Services.AddTransient<ExerciseDetailsPage>();

        builder.Services.AddSingleton<ExerciseService>();

        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<LoadingViewModel>();
        builder.Services.AddSingleton<ExerciseViewModel>();
        builder.Services.AddTransient<ExerciseDetailsViewModel>();

        return builder.Build();
	}
}
