using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ExerciseListPage : ContentPage
{
    private ExerciseViewModel viewModel;
    public ExerciseListPage(ExerciseViewModel exerciseViewModel)
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