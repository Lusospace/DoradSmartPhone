using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Views;
using System.ComponentModel;
using System.Windows.Input;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class DisplaySelectedItemsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IToast toast;
        private GlassDTO glassDTO;

        public ICommand ManualCommand => new Command(Manual);
        public ICommand AutomaticCommand => new Command(Automatic);

        public List<Widget> SelectedItems
        {
            get { return glassDTO.Widgets; }
            set
            {
                if (glassDTO.Widgets != value)
                {
                    glassDTO.Widgets = value;
                    OnPropertyChanged(nameof(SelectedItems));
                }
            }
        }

        public DisplaySelectedItemsViewModel(GlassDTO glassDTO, IToast toast)
        {
            Title = "Configuration Type";
            this.toast = toast;
            this.glassDTO = glassDTO;
        }

        public void Manual()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManualPage(glassDTO));
        }

        public void Automatic()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AutomaticPage(glassDTO, toast));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
