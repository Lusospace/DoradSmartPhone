using DoradSmartphone.Views;

namespace DoradSmartphone;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(ExerciseDetailsPage), typeof(ExerciseDetailsPage));
	}
}
