using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ManualPage : ContentPage
{
    private Point initialPosition;

    public ManualPage(TransferDTO transferDTO, IBluetoothService bluetoothService)
    {
        InitializeComponent();        
        BindingContext = new ManualViewModel(transferDTO, bluetoothService);
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var image = (Image)sender;
        const double sensitivity = 0.7;

        var ScreenSize = CalculateWidgetPositions.GetScreenResolution();

        var parentContainer = WidgetGrid; // Replace WidgetGrid with the actual parent container of the image

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                // Store the initial position of the image
                initialPosition = new Point(image.TranslationX, image.TranslationY);
                break;

            case GestureStatus.Running:
                // Move the image based on the pan offset with sensitivity control
                image.TranslationX = initialPosition.X + (e.TotalX * sensitivity);
                image.TranslationY = initialPosition.Y + (e.TotalY * sensitivity);
                break;

            case GestureStatus.Completed:
                // Get the final position of the image when the dragging is completed
                var finalPosition = new Point(image.TranslationX, image.TranslationY);
                initialPosition = finalPosition;

                // Calculate relative positions in percentage
                double relativeX = finalPosition.X / parentContainer.Width * 100.0;
                double relativeY = finalPosition.Y / parentContainer.Height * 100.0;

                // Calculate glasses positions (Unity-style)
                double glassesX = relativeX - 50.0;
                double glassesY = 50.0 - relativeY;

                Shell.Current.DisplayAlert("Warning", "The new glasses position is: X=" + glassesX + ", Y=" + glassesY, "Ok");
                break;
        }
    }
}