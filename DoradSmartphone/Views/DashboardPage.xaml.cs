using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class DashboardPage : ContentPage
{
    private ExerciseViewModel viewModel;
    public DashboardPage(ExerciseViewModel exerciseViewModel)
    {
        InitializeComponent();
        BindingContext = exerciseViewModel;
        viewModel = exerciseViewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.GetExerciseList();
    }
}