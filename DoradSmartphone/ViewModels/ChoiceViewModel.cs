using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;

namespace DoradSmartphone.ViewModels
{
    public partial class ChoiceViewModel : BaseViewModel
    {        
        private IBluetoothService bluetoothService;
        private ExerciseService exerciseService;

        public ChoiceViewModel( ExerciseService exerciseService)
        {
            Title = "Route Option";
            bluetoothService = ServiceLocator.Get<IBluetoothService>();
            this.exerciseService = exerciseService;            
        }        

        [RelayCommand]
        public void OldRoute()
        {
            var exerciseViewModel = new ExerciseViewModel(exerciseService, bluetoothService);

            // Navigate to the StartRunPage with the ExerciseViewModel instance
            Application.Current.MainPage.Navigation.PushAsync(new StartRunPage(exerciseViewModel));
        }

        public void NewRoute(bool result)
        {
            if (result)
            {
                // Create instances of GlassDTO, IToast, and IBluetoothService
                var transferDTO = new TransferDTO
                {
                    Avatar = new AvatarDTO(),
                    Route = new List<Route>(),
                    Widgets = new List<Widget>(),
                    Exercise = new Exercise()
                };

                // Navigate to the WidgetPage with the instances
                Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(transferDTO, bluetoothService));
            }
        }
    }
}
