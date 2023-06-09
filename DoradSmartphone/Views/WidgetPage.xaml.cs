using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class WidgetPage : ContentPage
{	
	public WidgetPage(WidgetViewModel widgetViewModel)
	{
		InitializeComponent();
		BindingContext = widgetViewModel;
    }
}