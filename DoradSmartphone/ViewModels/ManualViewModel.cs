using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class ManualViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private GlassDTO glassDTO;
        private TransferDTO transferDTO;
        private IBluetoothService bluetoothService;

        private double sliderValue;
        private string sliderLabel;

        private Widget draggedItem;
        private int draggedItemIndex;

        public ICommand DragStartedCommand => new RelayCommand<Widget>(DragStarted);
        public ICommand ItemDroppedCommand => new RelayCommand<Widget>(ItemDropped);


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
            get { return widgets; }
            set
            {
                if (widgets != value)
                {
                    widgets = value;
                    OnPropertyChanged(nameof(Widgets));
                }
            }
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

        private void DragStarted(Widget item)
        {
            draggedItem = item;
            draggedItemIndex = Widgets.IndexOf(draggedItem);
        }

        private void ItemDropped(Widget targetItem)
        {
            if (draggedItem != null)
            {
                int targetIndex = Widgets.IndexOf(targetItem);
                int draggedItemIndex = Widgets.IndexOf(draggedItem);

                if (targetIndex != -1)
                {
                    Widgets[draggedItemIndex] = targetItem;
                    Widgets[targetIndex] = draggedItem;
                    

                    // Update the UI
                    OnPropertyChanged(nameof(Widgets));
                    ManualPage.ForceLayout();

                    var reordenatedItens = new List<Widget>();
                    Widgets.ToList().ForEach(w => reordenatedItens.Add(w));

                    // Swap the Widgets in the transferDTO.Widgets list
                    transferDTO.Widgets.Clear();
                    transferDTO.Widgets.AddRange(reordenatedItens);                    
                }
                draggedItem = null;
            }
        }

        [RelayCommand]
        public void ReviewPage()
        {
            glassDTO = EntityToDto.Convertion(transferDTO);
            glassDTO.WidgetConfiguration = false;
            SendOverBluetooth(glassDTO);
            Application.Current.MainPage.Navigation.PushAsync(new ControlDevicePage());
        }

        private void SendOverBluetooth(GlassDTO glassDTO) => bluetoothService.Write(ConvertToJsonAndBytes.Convert(glassDTO));

        private void LoadAutomaticPage()
        {
            var updatedWidgets = CalculateWidgetPositions.LoadAutomaticPage(transferDTO, out ContentPage manualPage);
            ManualPage = manualPage;

            // Update the Widgets collection to match the updated list of widgets
            Widgets = new ObservableCollection<Widget>(updatedWidgets);

            // Update the GlassDTO with the modified Widgets
            transferDTO.Widgets = updatedWidgets.ToList();
        }

        private void UpdateSliderLabel()
        {
            int sliderValueInt = (int)SliderValue;

            // Use the converted integer value in the switch statement
            switch (sliderValueInt)
            {                
                case 1:
                    SliderLabel = "1.306329035";
                    break;
                case 2:
                    SliderLabel = "1.088607529";
                    break;
                case 3:
                    SliderLabel = "0.933092168";
                    break;
                case 4:
                    SliderLabel = "0.816455647";
                    break;
                case 5:
                    SliderLabel = "0.725738353";
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
