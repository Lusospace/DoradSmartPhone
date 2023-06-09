using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class WidgetViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<ImageWidget> _images;
        public ObservableCollection<ImageWidget> Images
        {
            get { return _images; }
            set
            {
                _images = value;
                OnPropertyChanged();
            }
        }

        public ICommand ItemSelectedCommand { get; }

        public WidgetViewModel()
        {
            Title = "Widget Selection";
            Images = new ObservableCollection<ImageWidget>();
            LoadImages();

            ItemSelectedCommand = new Command(ItemSelected);
        }

        private void LoadImages()
        {
            var imagePaths = Directory.GetFiles("pathToImagesFolder");

            foreach (var imagePath in imagePaths)
            {
                Images.Add(new ImageWidget { ImageSource = ImageSource.FromFile(imagePath), IsSelected = false });
            }
        }

        private void ItemSelected()
        {
            int selectedCount = Images.Count(i => i.IsSelected);

            if (selectedCount >= 1 && selectedCount <= 5)
            {
                // Move to the next page
                // Implement your navigation logic here
            }
            else
            {
                // Show an error message or handle the selection error
            }
        }

        // Implement INotifyPropertyChanged interface methods
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
