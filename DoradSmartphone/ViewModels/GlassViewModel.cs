using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
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
            status = "Disconnected";
            bluetoothService = ServiceLocator.Get<IBluetoothService>();            
            bluetoothService.ConnectionStatusChanged += BluetoothService_ConnectionStatusChanged;
            CheckConnection();
        }

        /// <summary>
        /// Verify the status of the connection as sonn as the app starts.
        /// </summary>
        public void CheckConnection() => Status = bluetoothService.CheckConnection() ? "Connected" : "Disconnected";

        /// <summary>
        /// Check the connection status if its's connected, it picks a photo from the smartphone album, 
        /// add into the commandDTO with the start debug command, send over bluetooth and go to the Calibration Page. 
        /// If it's not connected then just fire a Toast message
        /// </summary>
        /// <returns>Nothing</returns>
        /// <exception cref="Exception"></exception>
        [RelayCommand]
        public async Task Calibration()
        {
            var connection = bluetoothService.CheckConnection();

            //if (connection)
            //{
            byte[] photoData = await PhotoPickerHelper.PickPhotoAsync();

            if (photoData != null)
            {
                try
                {
                    CommandDTO command = new CommandDTO
                    {
                        Command = Constants.STARTDEBUG,
                        Image = photoData
                    };
                    SendOverBluetooth(command);
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

        /// <summary>
        /// Receive the command DTO, send it to the method ConvertToJsonAndBytes, and send the result over bluetooth.
        /// </summary>
        /// <param name="command"></param>
        private void SendOverBluetooth(CommandDTO command) => bluetoothService.Write(ConvertToJsonAndBytes.Convert(command));

        /// <summary>
        /// Event Listener to update the Status property based on the Bluetooth connection status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isConnected"></param>
        private void BluetoothService_ConnectionStatusChanged(object sender, bool isConnected)
        {            
            Status = isConnected ? "Connected" : "Disconnected";
        }
    }
}
