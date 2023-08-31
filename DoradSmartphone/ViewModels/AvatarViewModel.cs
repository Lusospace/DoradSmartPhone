using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Data;
using DoradSmartphone.DTO;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.ComponentModel;

namespace DoradSmartphone.ViewModels
{
    public partial class AvatarViewModel : BaseViewModel, INotifyPropertyChanged
    {        
        
        public TransferDTO transferDTO;        
        private IBluetoothService bluetoothService;

        public event PropertyChangedEventHandler PropertyChanged;

        private string speed;
        public string Speed
        {
            get { return speed; }
            set
            {
                if (speed != value)
                {
                    speed = value;
                    OnPropertyChanged(nameof(Speed));
                    UpdateNewSpeed();
                }
            }
        }

        private double routeSpeed;
        public double RouteSpeed
        {
            get { return routeSpeed; }
            set
            {
                if (routeSpeed != value)
                {
                    routeSpeed = value;
                    OnPropertyChanged(nameof(RouteSpeed));
                    UpdateNewSpeed();
                }
            }
        }

        private double newSpeed;
        public double NewSpeed
        {
            get { return newSpeed; }
            set
            {
                if (newSpeed != value)
                {
                    newSpeed = value;
                    OnPropertyChanged(nameof(NewSpeed));
                }
            }
        }

        private string percentage;
        public string Percentage
        {
            get { return percentage; }
            set
            {
                if (percentage != value)
                {
                    percentage = value;
                    OnPropertyChanged(nameof(Percentage));
                    UpdateNewSpeed();
                }
            }
        }

        public AvatarViewModel(TransferDTO transferDTO, IBluetoothService bluetoothService)
        {
            Title = "Avatar Page";            
            this.transferDTO = transferDTO;            
            this.bluetoothService = bluetoothService;
            RouteSpeed = transferDTO.Exercise.Speed != null ? transferDTO.Exercise.Speed.Avg : 0;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }         

        private void UpdateNewSpeed()
        {
            if (!string.IsNullOrWhiteSpace(percentage) && double.TryParse(percentage, out double percentageValue))
            {
                if (percentageValue >= 0)
                {
                    var calc = RouteSpeed + (RouteSpeed * (percentageValue / 100));
                    NewSpeed = double.Round(calc, 2, MidpointRounding.AwayFromZero);
                }
            } else if (!string.IsNullOrWhiteSpace(speed) && double.TryParse(speed, out double SpeedValue))
            {
                if (SpeedValue >= 0)
                {                    
                    NewSpeed = SpeedValue;
                }
            }
        }


        [RelayCommand]
        public void NextPage()
        {
            if (transferDTO.Avatar == null)
            {
                transferDTO.Avatar = new AvatarDTO(); // Create a new instance if null
            }
            transferDTO.Avatar.Speed = NewSpeed;
            transferDTO.Avatar.Active = true;
            Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(transferDTO, bluetoothService));
        }
    }
}
