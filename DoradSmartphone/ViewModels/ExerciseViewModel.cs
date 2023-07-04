using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
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
        }

        [ObservableProperty]
        bool isRefreshing;

        public async Task GetExerciseList()
        {
            if (IsLoading) return;
            try
            {
                IsLoading = true;
                if (Exercises.Any()) Exercises.Clear();

                var exercices = exerciseService.GetExercises();
                foreach (var exercise in exercices) Exercises.Add(exercise);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await Shell.Current.DisplayAlert("Error", "Failed to retrieve the exercice list", "Ok");
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
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
                        };
            }
            else
            {
                return new List<Location>
                        {
                            new Location(-1.4412474319742032, -48.485914192075455),
                            new Location(-1.4415280369316321, -48.48548039261385),
                            new Location(-1.438135265584208, -48.47889684784361),
                            new Location(-1.4519869242562538, -48.47759544945879),
                            new Location(-1.4515756786484433, -48.47169012644734),
                        };
            }
        }
    }
}
