﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.DTO;
using DoradSmartphone.Helpers;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.Views;
using Kotlin.Properties;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DoradSmartphone.ViewModels
{
    public partial class ExerciseViewModel : BaseViewModel, INotifyPropertyChanged
    {        
        private IBluetoothService bluetoothService;
        private List<Exercise> ExercisesList;
        private readonly ExerciseService exerciseService;
        private TransferDTO transferDTO;

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

        public ExerciseViewModel(ExerciseService exerciseService, IBluetoothService bluetoothService, TransferDTO transferDTO)
        {
            Title = "Training Routes";            
            this.exerciseService = exerciseService;
            this.bluetoothService = bluetoothService;
            this.transferDTO = transferDTO;

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
                    Toaster.MakeToast("User does not have exercises");
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
                Toaster.MakeToast("Failed to retrieve the exercice list " + ex.ToString());
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
                Toaster.MakeToast("Failed to retrieve the exercice list " + ex.ToString());
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
        public void Next(Exercise exercise)
        {
            transferDTO.Exercise = exercise;
            transferDTO.Route = exercise.Route;                        

            if (transferDTO.Avatar.Active.Equals(true))
            {                
                Application.Current.MainPage.Navigation.PushAsync(new AvatarPage(transferDTO, bluetoothService));
            } else
            {
                Application.Current.MainPage.Navigation.PushAsync(new WidgetPage(transferDTO, bluetoothService));
            }
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

        [RelayCommand]
        public void DeleteExercise(Exercise exercise)
        {
            try
            {
                if (ExercisesList != null && ExercisesList.Any())
                {
                    // Remove the exercise from the list
                    ExercisesList.Remove(exercise);
                    Exercises.Remove(exercise);
                    
                    exerciseService.DeleteExerciseAsync(exercise);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toaster.MakeToast("Failed to delete the exercise " + ex.ToString());
            }
        }

        [RelayCommand]
        public void ExerciseDetails(Exercise exercise)
        {
            try
            {
                if (ExercisesList != null && ExercisesList.Any())
                {
                    Application.Current.MainPage.Navigation.PushAsync(new ExerciseVisualizer(this, exercise));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Toaster.MakeToast("Failed to visualize the exercise " + ex.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
