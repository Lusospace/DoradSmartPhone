using AndroidX.Lifecycle;
using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class DisplaySelectedItemsPage : ContentPage
{    

    public DisplaySelectedItemsPage(List<Widget> selectedItems)
    {
        InitializeComponent();        
        BindingContext = new DisplaySelectedItemsViewModel(selectedItems);
    }
}