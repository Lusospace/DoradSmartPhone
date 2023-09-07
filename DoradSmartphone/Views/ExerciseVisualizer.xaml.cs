using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace DoradSmartphone.Views;

public partial class ExerciseVisualizer : ContentPage
{
    private ExerciseViewModel viewModel;
    private Exercise exercise;
    public ExerciseVisualizer(ExerciseViewModel exerciseViewModel, Exercise selectedExercise)
    {
        InitializeComponent();
        BindingContext = viewModel = exerciseViewModel;
        exercise = selectedExercise;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();        

        routeMap.MapElements.Clear();

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

        // Set exercise details labels
        startingDateLabel.Text = $"Starting Date: {exercise.StartingDate.ToString("dd/MM/yyyy HH:mm")}";
        timeLabel.Text = $"Time: {exercise.Time}";
        avgSpeedLabel.Text = $"Avg Speed: {exercise.Speed.Avg}";
        startingAddressLabel.Text = $"Starting Address: {exercise.StartingAddress}";
        finishingAddressLabel.Text = $"Finishing Address: {exercise.FinishingAddress}";
    }
}