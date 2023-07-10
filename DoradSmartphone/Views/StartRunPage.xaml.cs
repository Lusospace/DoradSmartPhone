using AndroidX.Lifecycle;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class StartRunPage : ContentPage
{
    private ExerciseViewModel viewModel;
    public StartRunPage(ExerciseViewModel exerciseViewModel)
    {
		InitializeComponent();
        BindingContext = exerciseViewModel;
        viewModel = exerciseViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.LoadExercisesAsync();
    }
}