using CommunityToolkit.Mvvm.ComponentModel;

namespace DoradSmartphone.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        bool isLoading;

        public bool IsNotLoading
        {
            get
            {
                return !IsLoading;
            }
        }
    }
}
