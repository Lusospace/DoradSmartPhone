﻿using CommunityToolkit.Mvvm.ComponentModel;
using DoradSmartphone.Models;

namespace DoradSmartphone.ViewModels
{
    [QueryProperty(nameof(Exercise), "Exercise")]
    public partial class ExerciseDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        Exercise exercise;

        public ExerciseDetailsViewModel()
        {            
        }
    }
}
