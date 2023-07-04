using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DoradSmartphone.ViewModels
{
    public partial class ManualViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private double sliderValue;
        private string sliderLabel;

        public double SliderValue
        {
            get { return sliderValue; }
            set
            {
                if (sliderValue != value)
                {
                    sliderValue = value;
                    OnPropertyChanged("SliderValue");
                    UpdateSliderLabel();
                }
            }
        }

        public string SliderLabel
        {
            get { return sliderLabel; }
            set
            {
                if (sliderLabel != value)
                {
                    sliderLabel = value;
                    OnPropertyChanged("SliderLabel");
                }
            }
        }

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
            SliderValue = 1;
            UpdateSliderLabel();
            Widgets = new ObservableCollection<Widget>(selectedItems);
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

        private void UpdateSliderLabel()
        {
            int sliderValueInt = (int)SliderValue;

            // Use the converted integer value in the switch statement
            switch (sliderValueInt)
            {
                case 1:
                    SliderLabel = "6.531645173";
                    break;
                case 2:
                    SliderLabel = "3.265822586";
                    break;
                case 3:
                    SliderLabel = "2.177215058";
                    break;
                case 4:
                    SliderLabel = "1.632911293";
                    break;
                case 5:
                    SliderLabel = "1.306329035";
                    break;
                case 6:
                    SliderLabel = "1.088607529";
                    break;
                case 7:
                    SliderLabel = "0.933092168";
                    break;
                case 8:
                    SliderLabel = "0.816455647";
                    break;
                case 9:
                    SliderLabel = "0.725738353";
                    break;
                case 10:
                    SliderLabel = "0.653164517";
                    break;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected new virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
