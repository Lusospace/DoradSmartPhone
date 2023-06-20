using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DoradSmartphone.ViewModels
{
    public partial class ManualViewModel : BaseViewModel, INotifyPropertyChanged
    {
        //public ICommand LoadAutomaticPageCommand => new Command(LoadAutomaticPage);

        private Widget draggedWidget;

        private int selectedWidget;

        private ObservableCollection<Widget> widgets;
        public ObservableCollection<Widget> Widgets
        {
            get => widgets;
            set => SetProperty(ref widgets, value);
        }

        private ContentPage automaticPage;
        public ContentPage AutomaticPage
        {
            get => automaticPage;
            set => SetProperty(ref automaticPage, value);
        }        

        public ManualViewModel(List<Widget> selectedItems)
        {
            Title = "Manual Configuration";
            Widgets = new ObservableCollection<Widget>(selectedItems);
        }

        [RelayCommand]
        public void Drag(Widget widget)
        {
            draggedWidget = widget;
        }

        [RelayCommand]
        public void Drop(string option)
        {
            int op = Convert.ToInt32(option);
            if (selectedWidget == op) return;

            if(draggedWidget != null)
            {
                var currentWidget = widgets[selectedWidget];               
            }
        }

        [RelayCommand]
        private void LoadAutomaticPage()
        {
            var layout = new Grid();

            int numColumns = (int)Math.Ceiling(Math.Sqrt(Widgets.Count));
            int numRows = (int)Math.Ceiling((double)Widgets.Count / numColumns);

            for (int row = 0; row < numRows; row++)
            {
                layout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int col = 0; col < numColumns; col++)
            {
                layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            int widgetIndex = 0;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    if (widgetIndex < Widgets.Count)
                    {
                        var widget = Widgets[widgetIndex];
                        var image = new Image
                        {
                            Source = widget.FileName,
                            Aspect = Aspect.AspectFit
                        };

                        Grid.SetRow(image, row);
                        Grid.SetColumn(image, col);
                        layout.Children.Add(image);

                        widgetIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            AutomaticPage = new ContentPage
            {
                Title = "Automatic",
                Content = layout
            };
        }

    }
}
