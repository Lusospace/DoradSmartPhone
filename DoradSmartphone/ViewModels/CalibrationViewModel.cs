using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.ComponentModel;

namespace DoradSmartphone.ViewModels
{
    public partial class CalibrationViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IBluetoothService bluetoothService;

        public event PropertyChangedEventHandler PropertyChanged;

        // Inside your ViewModel class
        private int _brightnessValue;
        public int BrightnessValue
        {
            get => _brightnessValue;
            set
            {
                if (_brightnessValue != value)
                {
                    _brightnessValue = value;
                    OnPropertyChanged(nameof(BrightnessValue));
                }
            }
        }

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

        /// <summary>
        /// Receives the selected image in the previous page and updates 
        /// the image in the Calibration Page
        /// </summary>
        /// <param name="photo"></param>
        private void LoadImage(byte[] photo)
        {
            if (photo != null)
            {
                SelectedPhoto = ImageSource.FromStream(() => new MemoryStream(photo));
            }
        }

        /// <summary>
        /// Send the command to stop the calibration mode
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task StopCalibration()
        {
            int connectionState = bluetoothService.GetState();
            if (connectionState == BluetoothService.STATE_CONNECTED)
            {
                CommandDTO command = new CommandDTO
                {
                    Command = Constants.STOPDEBUG,
                    Image = null
                };
                SendOverBluetooth(command);
                await GoToGlassPage();
            }
            else
            {
                Toaster.MakeToast("Bluetooth connection was lost.");
                await GoToGlassPage();
            }
        }

        /// <summary>
        /// Send the command to controll the brightness
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task SendBrightness()
        {
            int connectionState = bluetoothService.GetState();
            if (connectionState == BluetoothService.STATE_CONNECTED)
            {
                CommandDTO command = new CommandDTO
                {
                    Command = Constants.BRIGHTNESS,
                    Value = BrightnessValue
                };
                SendOverBluetooth(command);                
            }
            else
            {
                Toaster.MakeToast("Bluetooth connection was lost.");
                await GoToGlassPage();
            }
        }

        /// <summary>
        /// Picks another image from the device album to replace the previous one and 
        /// send to the glass initializing another debuging process
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        public async Task SwitchImage()
        {
            byte[] photoData = await PhotoPickerHelper.PickPhotoAsync();

            if (photoData != null)
            {
                SelectedPhoto = ImageSource.FromStream(() => new MemoryStream(photoData));
                SendImage(photoData);
            }
        }

        /// <summary>
        /// Receive the new image, check the Bluetooth connection, if it's connected, it prepare the 
        /// Command DTO with the start debug command and send it over Bluetooth. If there's no connection
        /// take the user back to the GlassPage
        /// </summary>
        /// <param name="photoData"></param>
        /// <exception cref="Exception"></exception>
        public async void SendImage(byte[] photoData)
        {
            try
            {
                int connectionState = bluetoothService.GetState();

                //if (connectionState == BluetoothService.STATE_CONNECTED)
                //{
                CommandDTO command = new CommandDTO
                {
                    Command = Constants.STARTDEBUG,
                    Image = photoData
                };
                SendOverBluetooth(command);
                //}
                //else
                //{
                //    Toaster.MakeToast("Bluetooth connection was lost.");
                //    await GoToGlassPage();
                //}
            }
            catch (Exception ex)
            {
                Toaster.MakeToast("Error when sending the image via bluetooth. " + ex);
                throw new Exception("Error when sending the image via bluetooth");
            }
        }

        /// <summary>
        /// Send the User to the GlassPage 
        /// </summary>
        /// <returns></returns>
        public async Task GoToGlassPage() => await Application.Current.MainPage.Navigation.PushAsync(new GlassPage());

        /// <summary>
        /// Convert the CommandDTO to bytes and send using bluetooth
        /// </summary>
        /// <param name="command"></param>
        private void SendOverBluetooth(CommandDTO command) => bluetoothService.Write(ConvertToJsonAndBytes.Convert(command));

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
