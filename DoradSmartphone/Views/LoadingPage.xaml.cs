using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(LoadingViewModel loadingPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = loadingPageViewModel;
    }
}