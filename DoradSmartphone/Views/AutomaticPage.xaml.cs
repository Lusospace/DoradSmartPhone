using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class AutomaticPage : ContentPage
{
    public AutomaticPage(List<Widget> selectedItems)
    {
        InitializeComponent();
        BindingContext = new AutomaticViewModel(selectedItems);
    }
}