using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class GlassViewModel : BaseViewModel
    {
        [ObservableProperty]
        private string battery;
        [ObservableProperty]
        private string status;        

        public GlassViewModel()
        {
            Title = "Glasses Page";            
            CheckConnection();
        }

        public void CheckConnection()
        {
            BluetoothService bluetoothService = new BluetoothService();

            bluetoothService.Start();

            int connectionState = bluetoothService.GetState();

            if (connectionState == BluetoothService.STATE_CONNECTED)
            {
                Status = "Connected";
            }
            else
            {
                Status = "Disconnected";
            }
        }

        [RelayCommand]
        public async Task Calibration()
        {
            CheckConnection();

            byte[] photoData = await PickPhotoAsync();

            if (photoData != null)
            {
                try
                {
                    //bluetoothService.Write(photoData);
                    _ = Application.Current.MainPage.Navigation.PushAsync(new CalibrationPage(photoData));
                }
                catch (Exception ex)
                {
                    Toaster.MakeToast("Error when sending the image via bluetooth. " + ex);
                    throw new Exception("Error when sending the image via bluetooth");
                }
            }
        }

        private async Task<byte[]> PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();
                if (photo != null)
                {
                    using (var stream = await photo.OpenReadAsync())
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Toaster.MakeToast("Error when picking image. " + ex);
                throw new Exception("Error picking image.");
            }

            return null;
        }

    }
}
