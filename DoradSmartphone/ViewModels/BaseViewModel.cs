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
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotScanning))]
        bool isScanning;

        public bool IsNotLoading => !IsLoading;
        public bool IsNotScanning => !IsScanning;
    }
}