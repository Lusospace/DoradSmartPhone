using DoradSmartphone.ViewModels;

namespace DoradSmartphone;

public partial class MainPage : ContentPage
{
    public MainPage(ExerciseViewModel exerciseViewModel)
    {
        InitializeComponent();
        BindingContext = exerciseViewModel;        
    }
}

