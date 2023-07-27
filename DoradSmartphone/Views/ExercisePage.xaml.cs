using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DoradSmartphone.Views;

public partial class ExercisePage : ContentPage
{

    private ExerciseViewModel viewModel;

    public ExercisePage(ExerciseViewModel exerciseViewModel)
    {
        InitializeComponent();
        BindingContext = exerciseViewModel;
        viewModel = exerciseViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = viewModel.GetExerciseList();
    }
    
    private void OnTapGestureRouteUpdate(object sender, EventArgs e)
    {
        routeMap.MapElements.Clear();

        var frame = (Frame)sender;
        var exercise = (Exercise)frame.BindingContext;
        var exerciseId = exercise.Id;
        var routes = viewModel.GetLocations(exerciseId);

        var polyline = new Polyline
        {
            StrokeColor = Colors.Red,
            StrokeWidth = 12,
        };

        foreach (var route in routes)
        {
            polyline.Geopath.Add(route);
        }

        routeMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                routes.FirstOrDefault(), Distance.FromMiles(5)));
        routeMap.MapElements.Add(polyline);
    }
}