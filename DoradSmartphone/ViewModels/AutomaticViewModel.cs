using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class AutomaticViewModel : BaseViewModel
    {
        private IToast toast;
        private GlassDTO glassDTO;
        private IBluetoothService bluetoothService;

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

        public AutomaticViewModel(GlassDTO glassDTO, IToast toast, IBluetoothService bluetoothService)
        {
            Title = "Automatic Configuration";
            this.toast = toast;
            this.glassDTO = glassDTO;
            this.bluetoothService = bluetoothService;
            Widgets = new ObservableCollection<Widget>(glassDTO.Widgets);
            LoadAutomaticPage();
        }

        [RelayCommand]
        public void ReviewPage()
        {
            SendOverBluetooth();
            Application.Current.MainPage.Navigation.PushAsync(new GeneralPage(glassDTO));
        }

        private void SendOverBluetooth() => bluetoothService.Write(ConvertToJsonAndBytes.Convert(glassDTO));
       

        private void LoadAutomaticPage()
        {
            var layout = new Grid();

            int numColumns = (int)Math.Ceiling(Math.Sqrt(glassDTO.Widgets.Count));
            int numRows = (int)Math.Ceiling((double)glassDTO.Widgets.Count / numColumns);

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
                    if (widgetIndex < glassDTO.Widgets.Count)
                    {
                        var widget = glassDTO.Widgets[widgetIndex];
                        var image = new Image
                        {
                            Source = widget.FileName,
                            Aspect = Aspect.AspectFit
                        };

                        Grid.SetRow(image, row);
                        Grid.SetColumn(image, col);
                        layout.Children.Add(image);

                        // Calculate relative positions
                        double relativeX = (col + 0.5) / numColumns; // Center of the column
                        double relativeY = (row + 0.5) / numRows; // Center of the row
                        widget.RelativeXPosition = relativeX;
                        widget.RelativeYPosition = relativeY;

                        // Calculate actual positions
                        double xPosition = col * (layout.Width / numColumns);
                        double yPosition = row * (layout.Height / numRows);
                        widget.XPosition = xPosition;
                        widget.YPosition = yPosition;

                        widgetIndex++;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            FormatPositions();

            AutomaticPage = new ContentPage
            {
                Title = "Automatic",
                Content = layout
            };

            // Update the GlassDTO with the modified Widgets
            glassDTO.Widgets = Widgets.ToList();
        }

        private void FormatPositions()
        {
            foreach (var widget in glassDTO.Widgets)
            {
                widget.RelativeXPosition = Math.Round(widget.RelativeXPosition, 2);
                widget.RelativeYPosition = Math.Round(widget.RelativeYPosition, 2);
                widget.XPosition = Math.Round(widget.XPosition, 2);
                widget.YPosition = Math.Round(widget.YPosition, 2);
            }
        }
    }
}
