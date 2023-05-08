using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ExercisePage : ContentPage
{
    public ExercisePage(ExerciseViewModel exerciseViewModel)
    {
        InitializeComponent();
        BindingContext = exerciseViewModel;
    }
}