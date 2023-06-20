using AndroidX.Lifecycle;
using DoradSmartphone.Models;
using DoradSmartphone.Views;
using System.ComponentModel;
using System.Windows.Input;
using static Android.App.Assist.AssistStructure;

namespace DoradSmartphone.ViewModels
{
    public partial class DisplaySelectedItemsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private List<Widget> _selectedItems;

        public ICommand ManualCommand => new Command(Manual);
        public ICommand AutomaticCommand => new Command(Automatic);

        public List<Widget> SelectedItems
        {
            get { return _selectedItems; }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    OnPropertyChanged(nameof(SelectedItems));
                }
            }
        }

        public DisplaySelectedItemsViewModel(List<Widget> selectedItems)
        {
            Title = "Configuration Type";
            SelectedItems = selectedItems;
        }

        public void Manual()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManualPage(SelectedItems));
        }

        public void Automatic()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AutomaticPage(SelectedItems));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
