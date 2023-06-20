using AndroidX.Lifecycle;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class WidgetPage : ContentPage
{
    private readonly WidgetViewModel viewModel;

    public WidgetPage(WidgetViewModel widgetViewModel)
	{
		InitializeComponent();
		BindingContext = widgetViewModel;
        viewModel = widgetViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.GetWidgetList();
    }
}