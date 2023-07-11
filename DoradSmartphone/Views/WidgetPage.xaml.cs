using AndroidX.Lifecycle;
using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class WidgetPage : ContentPage
{
    private readonly WidgetViewModel viewModel;

    public WidgetPage(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
    {
        InitializeComponent();
        var widgetViewModel = new WidgetViewModel(glassDTO, toast, bluetoothService);
        BindingContext = widgetViewModel;
        viewModel = widgetViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.GetWidgetList();
    }
}
