using Android.Gms.Maps.Model;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.ComponentModel;
using System.Windows.Input;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class AvatarViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private double speed;
        private double routeSpeed;
        private double newSpeed;
        private double percentage;

        private GlassDTO glassDTO;
        private IToast toast;
        private IBluetoothService bluetoothService;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Speed
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

        public double Percentage
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

        public AvatarViewModel(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
        {
            Title = "Avatar Page";
            this.glassDTO = glassDTO;
            this.toast = toast;
            this.bluetoothService = bluetoothService;
            RouteSpeed = glassDTO.Exercise.Speed.Avg;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateNewSpeed()
        {
            if (percentage != 0 || percentage >= 0)
            {
                var calc = RouteSpeed + (RouteSpeed * (Percentage / 100));
                NewSpeed = double.Round(calc, 2, MidpointRounding.AwayFromZero);
            }            
        }

        [RelayCommand]
        public void NextPage()
        {
            if (glassDTO.Avatar == null)
            {
                glassDTO.Avatar = new AvatarDTO(); // Create a new instance if null
            }
            glassDTO.Avatar.Speed = NewSpeed;
            glassDTO.Avatar.Active = true;
            Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(glassDTO, toast, bluetoothService));
        }
    }
}
