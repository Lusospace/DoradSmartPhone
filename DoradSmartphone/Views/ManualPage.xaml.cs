using DoradSmartphone.DTO;
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
        const double sensitivity = 0.7; // Adjust the sensitivity factor as needed

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

                // Calculate the real pixel position based on the total resolution of the screen
                var screenWidth = ScreenSize.Item1;
                var screenHeight = ScreenSize.Item2;

                var screenPositionX = (screenWidth * parentContainer.Width / parentContainer.Width) + finalPosition.X;
                var screenPositionY = (screenHeight * parentContainer.Height / parentContainer.Height) + finalPosition.Y;

                Shell.Current.DisplayAlert("Warning", "The new screen position is: X=" + screenPositionX + ", Y=" + screenPositionY, "Ok");
                break;
        }
    }

}