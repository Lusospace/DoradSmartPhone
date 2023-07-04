using DoradSmartphone.Models;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class AutomaticViewModel : BaseViewModel
    {
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

        public ICommand LoadAutomaticPageCommand => new Command(LoadAutomaticPage);

        public AutomaticViewModel(List<Widget> selectedItems)
        {
            Title = "Automatic Configuration";
            Widgets = new ObservableCollection<Widget>(selectedItems);
        }

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
