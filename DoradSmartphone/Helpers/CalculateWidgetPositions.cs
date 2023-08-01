using Android.Content.Res;
using Android.OS;
using Android.Util;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;

public static class CalculateWidgetPositions
{
    private static Tuple<int, int> ScreenSize;

    /// <summary>
    /// Receives the TransferDTO (Could it be only the Widget entity?!) and the ContentPage (Automatic or Manual Pages) then iterates through the widgets to calculate their position in the screen. 
    /// </summary>
    /// <param name="transferDTO"></param>
    /// <param name="widgetPage"></param>
    /// <returns>List of widgets </returns>
    public static List<Widget> LoadAutomaticPage(TransferDTO transferDTO, out ContentPage widgetPage)
    {
        GetScreenResolution();

        var layout = new Grid();
        var updatedWidgets = new List<Widget>();

        int numWidgets = transferDTO.Widgets.Count;
        int screenWidth = ScreenSize.Item1;
        int screenHeight = ScreenSize.Item2;

        double widgetWidth = Constants.widgetWidth; // Original width of the widget in pixels
        double widgetHeight = Constants.widgetHeight; // Original height of the widget in pixels

        double targetWidth = Constants.targetWidgetWidth; // Target width in XAML
        double targetHeight = Constants.targetWidgetHeight; // Target height in XAML

        for (int widgetIndex = 0; widgetIndex < numWidgets; widgetIndex++)
        {
            var widget = transferDTO.Widgets[widgetIndex];
            var image = new Image
            {
                Source = widget.FileName,
                Aspect = Aspect.AspectFit,
                WidthRequest = targetWidth,
                HeightRequest = targetHeight
            };

            layout.Children.Add(image);

            // Calculate actual positions
            double scale = Math.Min(targetWidth / widgetWidth, targetHeight / widgetHeight);
            double scaledWidth = widgetWidth * scale;
            double scaledHeight = widgetHeight * scale;

            double xPosition;
            double yPosition;

            if (widgetIndex == numWidgets - 1)
            {
                // Middle widget
                xPosition = (screenWidth - scaledWidth) / 2;
                yPosition = (screenHeight - scaledHeight) / 2;
            }
            else
            {
                int row = (widgetIndex < 2) ? 0 : 1;
                int column = (widgetIndex % 2 == 0) ? 0 : 1;

                xPosition = column * (screenWidth - scaledWidth);
                yPosition = row * (screenHeight - scaledHeight);
            }

            widget.XPosition = xPosition;
            widget.YPosition = yPosition;

            updatedWidgets.Add(widget);
        }

        CalculateRelativePositions(updatedWidgets);
        CalculateGlassesPositions(updatedWidgets);

        FormatPositions(updatedWidgets);

        widgetPage = new ContentPage
        {
            Title = "Widget Configuration",
            Content = layout
        };

        return updatedWidgets;
    }


    /// <summary>
    /// Calculate the relative positions of the widgets. The Calculation is based on cartesian plan (X and Y axis) divided by the screen size and multiply per 100
    /// </summary>
    /// <param name="widgets"></param>
    public static void CalculateRelativePositions(List<Widget> widgets)
    {
        foreach (var widget in widgets)
        {
            // Calculate relative positions in percentage
            double relativeX = widget.XPosition / ScreenSize.Item1 * 100.0;
            double relativeY = widget.YPosition / ScreenSize.Item2 * 100.0;
            widget.RelativeXPosition = relativeX;
            widget.RelativeYPosition = relativeY;
        }
    }

    /// <summary>
    /// Calculate the glass position based in the relative position in the smartphone device. 
    /// Taking in mind that Unity position metrics works differently from the cartesian plan, where the 0,0 position is in the middle of the screen
    /// </summary>
    /// <param name="widgets"></param>
    public static void CalculateGlassesPositions(List<Widget> widgets)
    {
        foreach (var widget in widgets)
        {
            // Calculate glasses positions
            double glassesX = widget.RelativeXPosition - 50.0;
            double glassesY = 50.0 - widget.RelativeYPosition;

            widget.GlassXPosition = glassesX;
            widget.GlassYPosition = glassesY;
        }
    }

    /// <summary>
    /// Here we just format to double with two digits
    /// </summary>
    /// <param name="widgets"></param>
    private static void FormatPositions(List<Widget> widgets)
    {
        foreach (var widget in widgets)
        {
            widget.RelativeXPosition = Math.Round(widget.RelativeXPosition, 2);
            widget.RelativeYPosition = Math.Round(widget.RelativeYPosition, 2);
            widget.GlassXPosition = Math.Round(widget.GlassXPosition, 2);
            widget.GlassYPosition = Math.Round(widget.GlassYPosition, 2);
            widget.XPosition = Math.Round(widget.XPosition, 2);
            widget.YPosition = Math.Round(widget.YPosition, 2);
        }
    }

    /// <summary>
    /// Method to get the screen resolution of the actual device, returns a Tuple of integer in which are the heigh and width of the pixel resolution
    /// </summary>
    /// <returns></returns>
    public static Tuple<int, int> GetScreenResolution()
    {
        var displayMetrics = new DisplayMetrics();
        var windowManager = Platform.CurrentActivity?.Window?.WindowManager;

        if (windowManager != null)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
            {
                var display = windowManager.DefaultDisplay;
                display.GetRealMetrics(displayMetrics);
            }
            else
            {
                windowManager.DefaultDisplay.GetMetrics(displayMetrics);
            }
        }
        else
        {
            displayMetrics = Resources.System.DisplayMetrics;
        }

        int widthPixels = displayMetrics.WidthPixels;
        int heightPixels = displayMetrics.HeightPixels;

        ScreenSize = Tuple.Create(widthPixels, heightPixels);

        return ScreenSize;
    }
}
