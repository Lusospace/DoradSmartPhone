using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views
{
    public partial class ChoicePage : ContentPage
    {
        private ChoiceViewModel viewModel;

        public ChoicePage(IBluetoothService bluetoothService, ExerciseService exerciseService)
        {
            InitializeComponent();
            viewModel = new ChoiceViewModel(bluetoothService, exerciseService);
            BindingContext = viewModel;
        }

        private async void NewRouteButton_Clicked(object sender, EventArgs e)
        {
            // Show a modal with the "In a New Route..." message and OK/Cancel buttons
            var result = await DisplayAlert("Attention", "In a New Route, the Avatar feature won't be available!", "OK", "Cancel");

            if (result)
            {
                viewModel.NewRoute(result);
            }
        }
    }
}
