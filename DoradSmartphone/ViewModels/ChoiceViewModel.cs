using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class ChoiceViewModel : BaseViewModel
    {
        private IToast toast;
        private IBluetoothService bluetoothService;
        private ExerciseService exerciseService;

        public ChoiceViewModel(IToast toast, IBluetoothService bluetoothService, ExerciseService exerciseService)
        {
            Title = "Route Option";
            this.toast = toast;
            this.bluetoothService = bluetoothService;
            this.exerciseService = exerciseService;            
        }        

        [RelayCommand]
        public void OldRoute()
        {
            var exerciseViewModel = new ExerciseViewModel(exerciseService, toast, bluetoothService);

            // Navigate to the StartRunPage with the ExerciseViewModel instance
            Application.Current.MainPage.Navigation.PushAsync(new StartRunPage(exerciseViewModel));
        }

        public void NewRoute(bool result)
        {
            if (result)
            {
                // Create instances of GlassDTO, IToast, and IBluetoothService
                var glassDto = new GlassDTO
                {
                    Avatar = null,
                    Exercise = null,
                    Widgets = null
                };

                // Navigate to the WidgetPage with the instances
                Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(glassDto, toast, bluetoothService));
            }
        }
    }
}
