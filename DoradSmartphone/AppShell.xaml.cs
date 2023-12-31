﻿using CommunityToolkit.Mvvm.Input;
using DoradSmartphone.ViewModels;
using DoradSmartphone.Views;

namespace DoradSmartphone;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        BindingContext= new AppShellViewModel();

       var getUserLogKey = Preferences.Get("UserLoggedIn", false);

        
        if (getUserLogKey == true)
        {
            DoradShell.CurrentItem = LogingShell;
        }
        else
        {
            DoradShell.CurrentItem = GlassShell;
        }

        Routing.RegisterRoute(nameof(GeneralPage), typeof(GeneralPage));
        
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Routing.RegisterRoute(nameof(UserPage), typeof(UserPage));       
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(GlassPage), typeof(GlassPage));
        Routing.RegisterRoute(nameof(ChoicePage), typeof(ChoicePage));
        Routing.RegisterRoute(nameof(AvatarPage), typeof(AvatarPage));
        Routing.RegisterRoute(nameof(WidgetPage), typeof(WidgetPage));
        Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
        Routing.RegisterRoute(nameof(StartRunPage), typeof(StartRunPage));
        Routing.RegisterRoute(nameof(ExercisePage), typeof(ExercisePage));
        Routing.RegisterRoute(nameof(AutomaticPage), typeof(AutomaticPage));
        Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));        
        Routing.RegisterRoute(nameof(CalibrationPage), typeof(CalibrationPage));        
        Routing.RegisterRoute(nameof(ControlDevicePage), typeof(ControlDevicePage));
        Routing.RegisterRoute(nameof(DisplaySelectedItemsPage), typeof(DisplaySelectedItemsPage));
        Routing.RegisterRoute(nameof(ExerciseVisualizer), typeof(ExerciseVisualizer));

    }    
}
