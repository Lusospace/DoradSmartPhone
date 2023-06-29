using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class DashboardPage : ContentPage
{
    private readonly DashboardViewModel viewModel;

    public DashboardPage()
    {
        InitializeComponent();
        viewModel = new DashboardViewModel();
        BindingContext = viewModel;
    }
}