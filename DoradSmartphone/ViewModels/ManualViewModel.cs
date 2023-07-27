using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DoradSmartphone.ViewModels
{
    public partial class ManualViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private GlassDTO glassDTO;
        private TransferDTO transferDTO;
        private IBluetoothService bluetoothService;

        private double sliderValue;
        private string sliderLabel;

        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                if (sliderValue != value)
                {
                    sliderValue = value;
                    OnPropertyChanged("SliderValue");
                    UpdateSliderLabel();
                }
            }
        }

        public string SliderLabel
        {
            get { return sliderLabel; }
            set
            {
                if (sliderLabel != value)
                {
                    sliderLabel = value;
                    OnPropertyChanged("SliderLabel");
                }
            }
        }

        private ObservableCollection<Widget> widgets;
        public ObservableCollection<Widget> Widgets
        {
            get => widgets;
            set => SetProperty(ref widgets, value);
        }

        private ContentPage manualPage;
        public ContentPage ManualPage
        {
            get => manualPage;
            set => SetProperty(ref manualPage, value);
        }

        public ManualViewModel(TransferDTO transferDTO, IBluetoothService bluetoothService)
        {
            Title = "Manual Configuration";
            SliderValue = 1;            
            this.transferDTO = transferDTO;
            this.bluetoothService = bluetoothService;
            Widgets = new ObservableCollection<Widget>(transferDTO.Widgets);

            UpdateSliderLabel();
            LoadAutomaticPage();
        }

        [RelayCommand]
        public void ReviewPage()
        {
            glassDTO = EntityToDto.Convertion(transferDTO);
            glassDTO.WidgetConfiguration = false;
            SendOverBluetooth(glassDTO);
            Application.Current.MainPage.Navigation.PushAsync(new GeneralPage(transferDTO));
        }

        private void SendOverBluetooth(GlassDTO glassDTO) => bluetoothService.Write(ConvertToJsonAndBytes.Convert(glassDTO));

        private void LoadAutomaticPage()
        {
            CalculateWidgetPositions.LoadAutomaticPage(transferDTO, out ContentPage manualPage);
            ManualPage = manualPage;

            // Update the GlassDTO with the modified Widgets
            transferDTO.Widgets = Widgets.ToList();
        }

        private void UpdateSliderLabel()
        {
            int sliderValueInt = (int)SliderValue;

            // Use the converted integer value in the switch statement
            switch (sliderValueInt)
            {
                case 1:
                    SliderLabel = "6.531645173";
                    break;
                case 2:
                    SliderLabel = "3.265822586";
                    break;
                case 3:
                    SliderLabel = "2.177215058";
                    break;
                case 4:
                    SliderLabel = "1.632911293";
                    break;
                case 5:
                    SliderLabel = "1.306329035";
                    break;
                case 6:
                    SliderLabel = "1.088607529";
                    break;
                case 7:
                    SliderLabel = "0.933092168";
                    break;
                case 8:
                    SliderLabel = "0.816455647";
                    break;
                case 9:
                    SliderLabel = "0.725738353";
                    break;
                case 10:
                    SliderLabel = "0.653164517";
                    break;
            }
            UpdateWidgetZPosition(sliderLabel);
        }

        public void UpdateWidgetZPosition(string zPosition)
        {
            if (transferDTO != null)
            {
                foreach (var widget in transferDTO.Widgets)
                {
                    widget.ZPosition = double.Parse(zPosition);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected new virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
