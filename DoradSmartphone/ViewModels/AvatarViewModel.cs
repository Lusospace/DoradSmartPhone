using Android.Gms.Maps.Model;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class AvatarViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private double speed;
        private double routeSpeed;
        private double newSpeed;
        private double percentage;

        private GlassDTO _glassDTO;

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

        public AvatarViewModel(GlassDTO glassDTO)
        {
            Title = "Avatar Page";
            _glassDTO = glassDTO;
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
            if (_glassDTO.Avatar == null)
            {
                _glassDTO.Avatar = new AvatarDTO(); // Create a new instance if null
            }
            _glassDTO.Avatar.Speed = NewSpeed;
            _glassDTO.Avatar.Active = true;
            //Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(_glassDTO));
        }
    }
}
