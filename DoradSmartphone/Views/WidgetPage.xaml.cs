using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class WidgetPage : ContentPage
{
    private readonly WidgetViewModel viewModel;

    public WidgetPage(TransferDTO transferDTO, IBluetoothService bluetoothService)
    {
        InitializeComponent();
        var widgetViewModel = new WidgetViewModel(transferDTO, bluetoothService);
        BindingContext = widgetViewModel;
        viewModel = widgetViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.GetWidgetList();
    }
}
