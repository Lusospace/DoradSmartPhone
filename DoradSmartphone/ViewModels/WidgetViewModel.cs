using Android.Widget;
using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Models;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class WidgetViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public IToast toast;

        [ObservableProperty]
        private bool isRefreshing;
        

        public ObservableCollection<Widget> Widgets { get; private set; } = new();

        private List<Widget> selectedItems;

   

        public ICommand DisplaySelectedItemsCommand => new Command(DisplaySelectedItems);


        public WidgetViewModel(IToast toast)
        {
            Title = "Widget Selection";
            this.toast = toast;
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
            selectedItems = Widgets.Where(w => w.IsSelected).Take(3).ToList();
            if(selectedItems.Count < 1)
            {
                toast.MakeToast($"Must select more than 1 Widget");
                return;
            } else if (selectedItems.Count > 5)
            {
                toast.MakeToast($"Must not select more than 5 Widgets");
                return;
            } else
            {
                // Navigate to another page and pass the selected items
                // Here's an example using NavigationPage and pushing a new page onto the navigation stack
                Application.Current.MainPage.Navigation.PushAsync(new DisplaySelectedItemsPage(selectedItems));
            }            
        }


        public List<Widget> GetWidgets() => new List<Widget>
        {
            new Widget {
            Id = 1, Name = "Battery", FileName = "Images/Widgets/route.png"
            },
            new Widget {
            Id = 2, Name = "Time", FileName = "Images/Widgets/route.png"
            },
            new Widget {
            Id = 3, Name = "Route", FileName = "Images/Widgets/route.png"
            },
            new Widget {
            Id = 4, Name = "Distance", FileName = "Images/Widgets/route.png"
            },
            new Widget {
            Id = 5, Name = "Speed", FileName = "Images/Widgets/route.png"
            },
        };
    }
}
