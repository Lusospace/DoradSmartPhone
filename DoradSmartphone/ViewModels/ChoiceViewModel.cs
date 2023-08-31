using Android.Gms.Maps.Model;
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

        private bool _yesChecked;
        private bool _noChecked;
        private bool _oldChecked;
        private bool _newChecked;

        public bool YesChecked
        {
            get => _yesChecked;
            set => SetProperty(ref _yesChecked, value);
        }

        public bool NoChecked
        {
            get => _noChecked;
            set => SetProperty(ref _noChecked, value);
        }

        public bool OldChecked
        {
            get => _oldChecked;
            set => SetProperty(ref _oldChecked, value);
        }

        public bool NewChecked
        {
            get => _newChecked;
            set => SetProperty(ref _newChecked, value);
        }

        public ChoiceViewModel( ExerciseService exerciseService)
        {
            Title = "Route Option";
            _yesChecked = false;
            _noChecked = false;
            _oldChecked = false;
            _newChecked = false;
            this.exerciseService = exerciseService;
            bluetoothService = ServiceLocator.Get<IBluetoothService>();            
        }        

        [RelayCommand]
        public void NextButton()
        {
            var transferDTO = new TransferDTO
            {
                Avatar = new AvatarDTO(),
                Route = new List<Route>(),
                Widgets = new List<Widget>(),
                Exercise = new Exercise()
            };

            if (YesChecked && OldChecked)
            {
                transferDTO.Avatar.Active = true;
                ExercisePageChoice(transferDTO);
            }
            else if (YesChecked && NewChecked)
            {
                transferDTO.Avatar.Active = true;
                AvatarPageChoice(transferDTO);

            } else if (NoChecked && OldChecked)
            {
                transferDTO.Avatar.Active = false;
                ExercisePageChoice(transferDTO);
            }
            else
            {
                WidgetPageChoice(transferDTO);
            }
        }

        public void ExercisePageChoice(TransferDTO transferDTO)
        {
            var exerciseViewModel = new ExerciseViewModel(exerciseService, bluetoothService, transferDTO);
            Application.Current.MainPage.Navigation.PushAsync(new StartRunPage(exerciseViewModel));
        }

        public void WidgetPageChoice(TransferDTO transferDTO)
        {
            Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(transferDTO, bluetoothService));
        }
        public void AvatarPageChoice(TransferDTO transferDTO)
        {
            Application.Current.MainPage.Navigation.PushAsync(new AvatarPage(transferDTO, bluetoothService));
        }
    }
}