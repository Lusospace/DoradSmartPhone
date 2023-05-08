using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DoradSmartphone;
using DoradSmartphone.Models;
using DoradSmartphone.Services;
using DoradSmartphone.Views;
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

        [RelayCommand]
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
            
            await Shell.Current.GoToAsync(nameof(ExerciseDetailsPage), true, new Dictionary<string, object>
            {
                {nameof(Exercise), exercise }
            });
        }
    }
}
