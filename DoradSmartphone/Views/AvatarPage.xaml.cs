using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class AvatarPage : ContentPage
{
    public AvatarPage(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
    {
        InitializeComponent();
        BindingContext = new AvatarViewModel(glassDTO, toast, bluetoothService);
    }

    private void SpeedEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                Percentage.Text = string.Empty;
            }
        }
    }

    private void PercentageEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            if (!string.IsNullOrWhiteSpace(entry.Text))
            {
                Speed.Text = string.Empty;
            }
        }
    }
}