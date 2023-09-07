using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DoradSmartphone.Views;

public partial class ExercisePage : ContentPage
{

    private ExerciseViewModel viewModel;

    public ExercisePage(ExerciseViewModel exerciseViewModel)
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