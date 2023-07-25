using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.ComponentModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class DisplaySelectedItemsViewModel : BaseViewModel, INotifyPropertyChanged
    {        
        private TransferDTO transferDTO;
        private IBluetoothService bluetoothService;

        public ICommand ManualCommand => new Command(Manual);
        public ICommand AutomaticCommand => new Command(Automatic);

        public List<Widget> SelectedItems
        {
            get { return transferDTO.Widgets; }
            set
            {
                if (transferDTO.Widgets != value)
                {
                    transferDTO.Widgets = value;
                    OnPropertyChanged(nameof(SelectedItems));
                }
            }
        }

        public DisplaySelectedItemsViewModel(TransferDTO transferDTO, IBluetoothService bluetoothService)
        {
            Title = "Configuration Type";
            this.transferDTO = transferDTO;
            this.bluetoothService = bluetoothService;
        }

        public void Manual()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManualPage(transferDTO, bluetoothService));
        }

        public void Automatic()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AutomaticPage(transferDTO, bluetoothService));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
