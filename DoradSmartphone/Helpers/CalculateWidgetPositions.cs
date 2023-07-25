using Android.Content.Res;
using Android.OS;
using Android.Util;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using System.Reflection.Metadata;

public static class CalculateWidgetPositions
{
    private static Tuple<int, int> ScreenSize;

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
