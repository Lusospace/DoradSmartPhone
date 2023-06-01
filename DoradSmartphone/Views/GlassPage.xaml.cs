using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class GlassPage : ContentPage
{
	public GlassPage(GlassViewModel glassViewModel)
	{
        InitializeComponent();
		BindingContext = glassViewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }


    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}