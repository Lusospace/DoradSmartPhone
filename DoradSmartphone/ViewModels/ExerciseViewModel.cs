using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using Kotlin.Properties;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ToastProject;

namespace DoradSmartphone.ViewModels
{
    public partial class ExerciseViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private IToast toast;
        private IBluetoothService bluetoothService;
        private List<Exercise> ExercisesList;
        private readonly ExerciseService exerciseService;

        [ObservableProperty]
        string finalAddress;

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

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand { get; }

        public ExerciseViewModel(ExerciseService exerciseService, IToast toast, IBluetoothService bluetoothService)
        {
            Title = "Training Routes";
            this.toast = toast;
            this.exerciseService = exerciseService;
            this.bluetoothService = bluetoothService;

            RefreshCommand = new Command(async () =>
            {
                await RefreshData();                
            });
        }

        public async Task GetExerciseList()
        {
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
                foreach (var exercise in exercices)
                {
                    var lastRoute = exercise.Route[exercise.Route.Count - 1];

                    Exercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                toast.MakeToast("Failed to retrieve the exercice list " + ex.ToString());
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task RefreshData()
        {

            try
            {
                IsRefreshing = false;
                await GetExerciseList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                toast.MakeToast("Failed to retrieve the exercice list " + ex.ToString());
            }
            finally
            {
                IsRefreshing = false;
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

            Application.Current.MainPage.Navigation.PushAsync(new AvatarPage(dto, toast, bluetoothService));        
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
