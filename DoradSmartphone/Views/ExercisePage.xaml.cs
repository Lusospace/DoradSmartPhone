using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DoradSmartphone.Views;

public partial class ExercisePage : ContentPage
{
    public ExercisePage(ExerciseViewModel exerciseViewModel)
    {
        InitializeComponent();
        BindingContext = exerciseViewModel;        
    }

    
    private void OnTapGestureRouteUpdate(object sender, EventArgs e)
    {
        var somora = new Polyline
        {
            StrokeColor = Colors.Red,
            StrokeWidth = 12,
            Geopath =
            {
                new Location(38.70061856336034 , -8.957381918676203 ),
                new Location(38.70671683905933 , -8.945225024701308 ),
                new Location(38.701985630081595, -8.944503277546072 ),
                new Location(38.701872978433386, -8.940750192338834 ),
                new Location(38.71054663609023 , -8.939162348597312 ),
                new Location(38.717755109243214, -8.942193686649311 ),
                new Location(38.7435419727561  , -8.928480490699792 ),
                new Location(38.78327379379296 , -8.880556478454272 ),
                new Location(38.925473761602376, -8.881999972299806 ),
                new Location(38.93692729913667 , -8.869585920414709 ),
                new Location(38.93493556584553 , -8.86536198145887  )
            }
        };
        routeMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                new Location(38.93479161472441, -8.865352563545757), Distance.FromMiles(1)));
        // Add the polyline to the map
        routeMap.MapElements.Add(somora);
    }
}