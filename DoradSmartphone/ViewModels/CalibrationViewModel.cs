using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class CalibrationViewModel : BaseViewModel
    {
        private IBluetoothService bluetoothService;        

        public CalibrationViewModel(byte[] photo, IBluetoothService bluetoothService)
        {
            Title = "Calibration Page";
            this.bluetoothService = bluetoothService;
            LoadImage(photo);
        }

        private ImageSource _selectedPhoto;
        public ImageSource SelectedPhoto
        {
            get => _selectedPhoto;
            set => SetProperty(ref _selectedPhoto, value);
        }

        private void LoadImage(byte[] photo)
        {
            if (photo != null)
            {
                SelectedPhoto = ImageSource.FromStream(() => new MemoryStream(photo));
            }
        }

        [RelayCommand]
        public async Task StopCalibration() 
        {
            bluetoothService.Write(ConvertToJsonAndBytes.Convert(Constants.STOP));
            await GoToGlassPage();
        } 

        [RelayCommand]
        public async Task SwitchImage()
        {            
            byte[] photoData = await PhotoPickerHelper.PickPhotoAsync();

            if (photoData != null)
            {
                try
                {
                    SelectedPhoto = ImageSource.FromStream(() => new MemoryStream(photoData));
                    SendImage(photoData);
                }
                catch (Exception ex)
                {
                    Toaster.MakeToast("Error when sending the image via bluetooth. " + ex);
                    throw new Exception("Error when sending the image via bluetooth");
                }
            }
        }

        public async void SendImage(byte[] photoData)
        {
            int connectionState = bluetoothService.GetState();

            if (connectionState == BluetoothService.STATE_CONNECTED)
            {                
                bluetoothService.Write(photoData);            
            }
            else
            {
                Toaster.MakeToast("Bluetooth connection was lost.");
                await GoToGlassPage();
            }
        }

        public async Task GoToGlassPage() => await Application.Current.MainPage.Navigation.PushAsync(new GlassPage());        
    }
}
