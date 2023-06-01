using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Views;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace DoradSmartphone.ViewModels
{
    public partial class ExerciseViewModel : BaseViewModel
    {
        private readonly ExerciseService exerciseService;
        

        public ObservableCollection<Exercise> Exercises { get; private set; } = new();
        public ExerciseViewModel(ExerciseService exerciseService)
        {
            Title = "Training Routes";
            this.exerciseService = exerciseService;
            _ = GetExerciseList();                                   
        }

        [ObservableProperty]
        bool isRefreshing;
        
        async Task GetExerciseList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Exercises.Any()) Exercises.Clear();

                var exercices = exerciseService.GetExercises();
                foreach (var exercise in exercices) Exercises.Add(exercise);
            } catch(Exception ex) { 
                Console.WriteLine(ex.ToString());
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve the exercice list", "Ok");
            }
            finally { 
                IsLoading = false; 
                isRefreshing= false;
             }
        }
        [RelayCommand]
        async Task ExerciseDetails(Exercise exercise)
        {
            if(exercise == null) return;

            var routes = GetLocations(exercise.Id);

            DrawRoutes(routes);
        }

        public List<Location> GetLocations(int exerciseId)
        {
            if (exerciseId == 1)
            {
                return new List<Location>
                        {
                            new Location(35.6823324582143, 139.7620853729577),
                            new Location(35.679263477092704, 139.75773939496295),
                            new Location(35.68748054650018, 139.761486207315),
                            new Location(35.690745005825136, 139.7560362984393),
                            new Location(35.68966608916097, 139.75147199952355),
                            new Location(35.68427128680411, 139.7442168083328)
                        };
            }
            else if (exerciseId == 2)
            {
                return new List<Location>
                        {
                            new Location(35.6823324582143, 139.7620853729577),
                            new Location(35.679263477092704, 139.75773939496295),
                            new Location(35.68748054650018, 139.761486207315),
                            new Location(35.690745005825136, 139.7560362984393),
                            new Location(35.68966608916097, 139.75147199952355),
                            new Location(35.68427128680411, 139.7442168083328)
                        };
            }
            else
            {
                return new List<Location>
                        {
                            new Location(35.6823324582143, 139.7620853729577),
                            new Location(35.679263477092704, 139.75773939496295),
                            new Location(35.68748054650018, 139.761486207315),
                            new Location(35.690745005825136, 139.7560362984393),
                            new Location(35.68966608916097, 139.75147199952355),
                            new Location(35.68427128680411, 139.7442168083328)
                        };
            }
        }

        private void DrawRoutes(List<Location> routes)
        {
            var polylines = new Polyline
            {
                StrokeColor = Colors.Red,
                StrokeWidth = 12,
            };

            foreach(var route in routes)
            {
                polylines.Geopath.Add(route);
            }

            //myMap.MoveToRegion(
            //    MapSpan.FromCenterAndRadius(
            //        routes.FirstOrDefault(), Distance.FromMiles(1)));
            //// Add the polyline to the map
            //myMap.MapElements.Add(polylines);
        }
    }
}
