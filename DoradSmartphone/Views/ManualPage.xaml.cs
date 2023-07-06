using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using System.Drawing;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class ManualPage : ContentPage
{
    private Microsoft.Maui.Graphics.Point initialPosition;
    public ManualPage(List<Widget> selectedItems)
    {
        InitializeComponent();        
        BindingContext = new ManualViewModel(selectedItems);
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var image = (Image)sender;
        const double sensitivity = 0.7; // Adjust the sensitivity factor as needed

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                // Store the initial position of the image
                initialPosition = new Microsoft.Maui.Graphics.Point(image.TranslationX, image.TranslationY);
                break;

            case GestureStatus.Running:
                // Move the image based on the pan offset with sensitivity control
                image.TranslationX = initialPosition.X + (e.TotalX * sensitivity);
                image.TranslationY = initialPosition.Y + (e.TotalY * sensitivity);
                break;

            case GestureStatus.Completed:
                // Get the final position of the image when the dragging is completed
                var finalPosition = new Microsoft.Maui.Graphics.Point(image.TranslationX, image.TranslationY);
                initialPosition = finalPosition;
                Shell.Current.DisplayAlert("Warning", "The new position is: " + finalPosition, "Ok");
                
                var gridWidth = WidgetGrid.Width;
                var gridHeight = WidgetGrid.Height;

                // Calculate the percentage values
                var relativeX = (finalPosition.X / gridWidth) * 100;
                var relativeY = (finalPosition.Y / gridHeight) * 100;

                var formattedRelativeX = relativeX.ToString("0.00");
                var formattedRelativeY = relativeY.ToString("0.00");

                Shell.Current.DisplayAlert("Warning", "The new relative position is for the X: " + formattedRelativeX + " and for the Y " + formattedRelativeY, "Ok");
                break;
        }
    }   
}