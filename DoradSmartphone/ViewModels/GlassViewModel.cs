using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class GlassViewModel : BaseViewModel
    {
        private readonly IBluetoothService bluetoothService;

        [ObservableProperty]
        private string battery;
        [ObservableProperty]
        private string status;

        public GlassViewModel()
        {
            Title = "Glasses Page";
            bluetoothService = ServiceLocator.Get<IBluetoothService>();
            CheckConnection();
        }

        public bool CheckConnection()
        {
            try
            {
                bluetoothService.Start();

                int connectionState = bluetoothService.GetState();

                if (connectionState == BluetoothService.STATE_CONNECTED)
                {
                    Status = "Connected";
                    return true;
                }
                else
                {
                    Status = "Disconnected";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Toaster.MakeToast("Error " + ex);
                throw new Exception("Error when cheking the Dluetooth status.");
            }
        }

        [RelayCommand]
        public async Task Calibration()
        {
            var connection = CheckConnection();

            //if (connection)
            //{
                byte[] photoData = await PhotoPickerHelper.PickPhotoAsync();

                if (photoData != null)
                {
                    try
                    {
                        //bluetoothService.Write(photoData);
                        _ = Application.Current.MainPage.Navigation.PushAsync(new CalibrationPage(photoData, bluetoothService));
                    }
                    catch (Exception ex)
                    {
                        Toaster.MakeToast("Error when sending the image via bluetooth. " + ex);
                        throw new Exception("Error when sending the image via bluetooth");
                    }
                }
            //} else
            //{
            //    Toaster.MakeToast("No device paried.");
            //}
        }
    }
}
