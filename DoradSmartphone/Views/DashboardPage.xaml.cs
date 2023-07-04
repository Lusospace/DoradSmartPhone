using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class DashboardPage : ContentPage
{    
    public DashboardPage(DashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}