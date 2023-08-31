using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views
{
    public partial class ChoicePage : ContentPage
    {
        private ChoiceViewModel viewModel;

        public ChoicePage(ExerciseService exerciseService)
        {
            InitializeComponent();
            viewModel = new ChoiceViewModel(exerciseService);
            BindingContext = viewModel;
        }

        private void YesCheckBox_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                // If "Yes" checkbox is checked, uncheck the "No" checkbox
                NoCheckBox.IsChecked = false;
            }
        }

        private void NoCheckBox_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                // If "No" checkbox is checked, uncheck the "Yes" checkbox
                YesCheckBox.IsChecked = false;
            }
        }

        private void OldCheckBox_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                // If "Yes" checkbox is checked, uncheck the "No" checkbox
                NewCheckBox.IsChecked = false;
            }
        }

        private void NewCheckBox_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value)
            {
                // If "No" checkbox is checked, uncheck the "Yes" checkbox
                OldCheckBox.IsChecked = false;
            }
        }
    }
}
