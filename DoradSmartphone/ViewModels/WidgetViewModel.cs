﻿using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class WidgetViewModel : BaseViewModel, INotifyPropertyChanged
    {        
        private TransferDTO transferDTO;
        private IBluetoothService bluetoothService;        

        [ObservableProperty]
        private bool isRefreshing;

        public ObservableCollection<Widget> Widgets { get; private set; } = new();

        private List<Widget> selectedItems;

        public ICommand DisplaySelectedItemsCommand => new Command(DisplaySelectedItems);

        public WidgetViewModel(TransferDTO transferDTO, IBluetoothService bluetoothService)
        {
            Title = "Widget Selection";           
            this.transferDTO = transferDTO;
            this.bluetoothService = bluetoothService;
        }

        public async Task GetWidgetList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Widgets.Any()) Widgets.Clear();

                var widgets = GetWidgets();
                foreach (var widget in widgets) Widgets.Add(widget);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve the widgets list", "Ok");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        private void DisplaySelectedItems()
        {
            // Select a minimum of 1 and a maximum of 3 items
            selectedItems = Widgets.Where(w => w.IsSelected).ToList();
            if (selectedItems.Count < 1)
            {
                Toaster.MakeToast($"Must select at least 1 widget");
                return;
            }
            else if (selectedItems.Count > 5)
            {
                Toaster.MakeToast($"Must not select more than 5 widgets");
                return;
            }
            else
            {
                if (transferDTO.Widgets == null)
                {
                    transferDTO.Widgets = new List<Widget>();
                }
                transferDTO.Widgets = selectedItems;
                Application.Current.MainPage.Navigation.PushAsync(new DisplaySelectedItemsPage(transferDTO, bluetoothService));
            }
        }


        public List<Widget> GetWidgets() => new List<Widget>
        {
            new Widget {
            Id = 1, Name = "Battery", FileName = "Images/Widgets/battery.png"
            },
            new Widget {
            Id = 2, Name = "Time", FileName = "Images/Widgets/time.png"
            },
            new Widget {
            Id = 3, Name = "Route", FileName = "Images/Widgets/route.png"
            },
            new Widget {
            Id = 4, Name = "Distance", FileName = "Images/Widgets/distance.png"
            },
            new Widget {
            Id = 5, Name = "Speed", FileName = "Images/Widgets/kmh.png"
            },
            new Widget {
            Id = 6, Name = "Altitude", FileName = "Images/Widgets/altitude.png"
            },
            new Widget {
            Id = 7, Name = "Direction", FileName = "Images/Widgets/direction.png"
            },
            new Widget {
            Id = 8, Name = "Weather", FileName = "Images/Widgets/weather.png"
            },
            new Widget {
            Id = 9, Name = "Heart Rate", FileName = "Images/Widgets/heart.png"
            },
        };
    }
}
