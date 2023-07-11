using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class ExerciseViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IToast toast;
        private List<Exercise> ExercisesList;
        private readonly ExerciseService exerciseService;

        public ObservableCollection<Exercise> Exercises { get; private set; } = new();

        private ObservableCollection<Exercise> startExercises;
        public ObservableCollection<Exercise> StartExercises
        {
            get { return startExercises; }
            set
            {
                startExercises = value;
                OnPropertyChanged(nameof(StartExercises));
            }
        }

        public ExerciseViewModel(ExerciseService exerciseService, IToast toast)
        {
            Title = "Training Routes";
            this.toast = toast;
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

                var exercices = await exerciseService.RecoverExerciseByIdAsync();
                ExercisesList = exercices;
                if(exercices == null)
                {
                    toast.MakeToast("User does not have exercises");
                }
                foreach (var exercise in exercices) Exercises.Add(exercise);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                toast.MakeToast("Failed to retrieve the exercice list " + ex.ToString());
            }
            finally
            {
                IsLoading = false;
                IsRefreshing = false;
            }
        }

        public async Task LoadExercisesAsync()
        {
            var exercises = await exerciseService.RecoverExerciseByIdAsync();
            foreach (var exercise in exercises)
            {
                if (exercise.Route != null && exercise.Route.Count > 0)
                {
                    var firstRoute = exercise.Route[0];
                    string address = await GoogleMapsGeocoding.GetAddressName(firstRoute.Latitude, firstRoute.Longitude);
                    exercise.Route[0].Address = address;
                }

                Exercises.Add(exercise);
            }
        }


        [RelayCommand]
        public void Insert() => exerciseService.InsertExercises();

        [RelayCommand]
        public void Clear()
        {
            Exercises.Clear();
            exerciseService.ClearAll();
        }

        [RelayCommand]
        public void NavigateToAvatar(Exercise exercise)
        {
            // Create the DTO object
            var dto = new GlassDTO
            {
                Exercise = exercise,
                Widgets = null,                
                Avatar = null
            };

            Application.Current.MainPage.Navigation.PushAsync(new AvatarPage(dto, toast));        
        }

        public List<Location> GetLocations(int exerciseId)
        {
            List<Location> locations = new List<Location>();

            // Retrieve the Exercise entity based on the exerciseId
            var exercise = ExercisesList.FirstOrDefault(e => e.Id == exerciseId);
            if (exercise != null)
            {
                // Retrieve the Route entities associated with the Exercise
                var routes = exercise.Route;
                if (routes != null)
                {
                    // Convert the latitude and longitude information to Location objects
                    foreach (var route in routes)
                    {
                        Location location = new Location(route.Latitude, route.Longitude);
                        locations.Add(location);
                    }
                }
            }

            return locations;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
