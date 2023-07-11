using AndroidX.Lifecycle;
using DoradSmartphone.DTO;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class WidgetPage : ContentPage
{
    private readonly WidgetViewModel viewModel;

    public WidgetPage(GlassDTO glassDTO, IToast toast)
    {
        InitializeComponent();
        var widgetViewModel = new WidgetViewModel(glassDTO, toast);
        BindingContext = widgetViewModel;
        viewModel = widgetViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.GetWidgetList();
    }
}
