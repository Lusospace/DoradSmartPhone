using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class DashboardPage : ContentPage
{
	public DashboardPage(DashboardViewModel dashboardViewModel)
	{
		InitializeComponent();
		BindingContext = dashboardViewModel;
	}
}