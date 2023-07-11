using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class AutomaticViewModel : BaseViewModel
    {
        private IToast toast;
        private GlassDTO glassDTO;
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

        public AutomaticViewModel(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
        {
            Title = "Automatic Configuration";
            this.toast = toast;
            this.glassDTO = glassDTO;
            this.bluetoothService = bluetoothService;
            Widgets = new ObservableCollection<Widget>(glassDTO.Widgets);
            LoadAutomaticPage();
        }

        [RelayCommand]
        public void ReviewPage()
        {
            SendOverBluetooth();
            Application.Current.MainPage.Navigation.PushAsync(new GeneralPage(glassDTO));
        }

        private void SendOverBluetooth() => bluetoothService.Write(ConvertToJsonAndBytes.Convert(glassDTO));
       

        private void LoadAutomaticPage()
        {
            CalculateWidgetPositions.LoadAutomaticPage(glassDTO, out ContentPage automaticPage);
            AutomaticPage = automaticPage;

            // Update the GlassDTO with the modified Widgets
            glassDTO.Widgets = Widgets.ToList();
        }
    }
}
