using CommunityToolkit.Mvvm.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class CalibrationViewModel : BaseViewModel
    {
        public CalibrationViewModel(byte[] photo)
        {
            Title = "Calibration Page";
            LoadImage(photo);
        }

        private ImageSource _selectedPhoto;
        public ImageSource SelectedPhoto
        {
            get => _selectedPhoto;
            set => SetProperty(ref _selectedPhoto, value);
        }

        private void LoadImage(byte[] photo)
        {
            if (photo != null)
            {
                SelectedPhoto = ImageSource.FromStream(() => new MemoryStream(photo));
            }
        }

        [RelayCommand]
        public async Task StopCalibration()
        {
            // Implement the necessary logic to stop calibration.
        }
    }
}
