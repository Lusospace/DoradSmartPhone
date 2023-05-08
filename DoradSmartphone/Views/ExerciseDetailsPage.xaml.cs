using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ExerciseDetailsPage : ContentPage
{
	public ExerciseDetailsPage(ExerciseDetailsViewModel exerciseDetailsViewModel)
	{
		InitializeComponent();
		BindingContext= exerciseDetailsViewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}