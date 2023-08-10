using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class DisplaySelectedItemsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private TransferDTO transferDTO;
        private IBluetoothService bluetoothService;

        private Widget draggedItem;
        private int draggedItemIndex;

        public ICommand DragStartedCommand => new RelayCommand<Widget>(DragStarted);
        public ICommand ItemDroppedCommand => new RelayCommand<Widget>(ItemDropped);

        private void DragStarted(Widget item)
        {
            draggedItem = item;
            draggedItemIndex = SelectedItems.IndexOf(draggedItem);
        }

        private void ItemDropped(Widget targetItem)
        {
            if (draggedItem != null)
            {
                int targetIndex = SelectedItems.IndexOf(targetItem);
                int draggedItemIndex = SelectedItems.IndexOf(draggedItem);

                if (targetIndex != -1)
                {
                    SelectedItems[draggedItemIndex] = targetItem;
                    SelectedItems[targetIndex] = draggedItem;

                    // Update the UI
                    OnPropertyChanged(nameof(SelectedItems));

                    var reordenatedItens = new List<Widget>();

                    SelectedItems.ToList().ForEach(w => reordenatedItens.Add(w));

                    transferDTO.Widgets.Clear();
                    transferDTO.Widgets.AddRange(reordenatedItens);
                }
                draggedItem = null;
            }
        }

        private ObservableCollection<Widget> _selectedItems;
        public ObservableCollection<Widget> SelectedItems
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

        public DisplaySelectedItemsViewModel(TransferDTO transferDTO, IBluetoothService bluetoothService)
        {
            Title = "Widget Configuration";
            this.transferDTO = transferDTO;
            this.bluetoothService = bluetoothService;
            _selectedItems = new ObservableCollection<Widget>(transferDTO.Widgets);
        }

        [RelayCommand]
        public void Manual()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ManualPage(transferDTO, bluetoothService));
        }

        [RelayCommand]
        public void Automatic()
        {
            Application.Current.MainPage.Navigation.PushAsync(new AutomaticPage(transferDTO, bluetoothService));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}