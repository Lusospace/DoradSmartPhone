using DoradSmartphone.DTO;
using DoradSmartphone.Models;

public static class CalculateWidgetPositions
{
    public static List<Widget> LoadAutomaticPage(GlassDTO glassDTO, out ContentPage widgetPage)
    {
        var layout = new Grid();
        var updatedWidgets = new List<Widget>();

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

                    updatedWidgets.Add(widget);
                    widgetIndex++;
                }
                else
                {
                    break;
                }
            }
        }

        FormatPositions(updatedWidgets);

        widgetPage = new ContentPage
        {
            Title = "Widget Configuration",
            Content = layout
        };

        return updatedWidgets;
    }

    public static void FormatPositions(List<Widget> widgets)
    {
        foreach (var widget in widgets)
        {
            widget.RelativeXPosition = Math.Round(widget.RelativeXPosition, 2);
            widget.RelativeYPosition = Math.Round(widget.RelativeYPosition, 2);
            widget.XPosition = Math.Round(widget.XPosition, 2);
            widget.YPosition = Math.Round(widget.YPosition, 2);
        }
    }
}
