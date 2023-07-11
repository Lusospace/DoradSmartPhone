using AndroidX.Lifecycle;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;
using ToastProject;

namespace DoradSmartphone.Views;

public partial class DisplaySelectedItemsPage : ContentPage
{    

    public DisplaySelectedItemsPage(GlassDTO glassDTO, IToast toast)
    {
        InitializeComponent();        
        BindingContext = new DisplaySelectedItemsViewModel(glassDTO, toast);
    }
}