using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;

namespace DoradSmartphone.ViewModels
{
    public partial class AutomaticViewModel : BaseViewModel
    {
        private GlassDTO glassDTO;
        private TransferDTO transferDTO;
        private IBluetoothService bluetoothService;

        private ObservableCollection<Widget> widgets;
        public ObservableCollection<Widget> Widgets
        {
            get => widgets;
            set => SetProperty(ref widgets, value);
        }

        private ContentPage automaticPage;
        public ContentPage AutomaticPage
        {
            get => automaticPage;
            set => SetProperty(ref automaticPage, value);
        }        

        public AutomaticViewModel(TransferDTO transferDTO, IBluetoothService bluetoothService)
        {
            Title = "Automatic Configuration";            
            this.transferDTO = transferDTO;
            this.bluetoothService = bluetoothService;
            Widgets = new ObservableCollection<Widget>(transferDTO.Widgets);
            
            LoadAutomaticPage();
        }

        [RelayCommand]
        public void ReviewPage()
        {
            glassDTO = EntityToDto.Convertion(transferDTO);
            glassDTO.WidgetConfiguration = true;
            SendOverBluetooth(glassDTO);
            Application.Current.MainPage.Navigation.PushAsync(new GeneralPage(glassDTO));
        }

        private void SendOverBluetooth(GlassDTO glassDTO) => bluetoothService.Write(ConvertToJsonAndBytes.Convert(glassDTO));
       

        private void LoadAutomaticPage()
        {
            CalculateWidgetPositions.LoadAutomaticPage(transferDTO, out ContentPage automaticPage);
            AutomaticPage = automaticPage;

            // Update the GlassDTO with the modified Widgets
            transferDTO.Widgets = Widgets.ToList();
        }
    }
}
