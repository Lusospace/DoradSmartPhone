using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class ManualPage : ContentPage
{
    private Point initialPosition;
    public ManualPage(List<Widget> selectedItems)
    {
        InitializeComponent();        
        BindingContext = new ManualViewModel(selectedItems);
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        var image = (Image)sender;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                // Store the initial position of the image
                initialPosition = new Point(image.TranslationX, image.TranslationY);
                break;

            case GestureStatus.Running:
                // Move the image based on the pan offset
                image.TranslationX = initialPosition.X + e.TotalX;
                image.TranslationY = initialPosition.Y + e.TotalY;
                break;

            case GestureStatus.Completed:
                // Get the final position of the image when the dragging is completed
                var finalPosition = new Point(image.TranslationX, image.TranslationY);

                Shell.Current.DisplayAlert("Warning", "The position is: " + finalPosition, "Ok");
                // Perform any desired actions with the final position
                // ...
                break;
        }
    }
}