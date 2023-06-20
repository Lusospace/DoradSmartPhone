using DoradSmartphone.Models;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views;

public partial class ManualPage : ContentPage
{
	public ManualPage(List<Widget> selectedItems)
	{
        InitializeComponent();
        BindingContext = new ManualViewModel(selectedItems);
    }
}