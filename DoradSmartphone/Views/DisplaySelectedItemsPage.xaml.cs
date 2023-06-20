using DoradSmartphone.Models;

namespace DoradSmartphone.Views;

public partial class DisplaySelectedItemsPage : ContentPage
{
    public DisplaySelectedItemsPage(List<Widget> selectedItems)
    {
        InitializeComponent();

        var listView = new ListView
        {
            ItemsSource = selectedItems
        };

        var dataTemplate = new DataTemplate(() =>
        {
            var nameLabel = new Label();
            nameLabel.SetBinding(Label.TextProperty, "Name");

            return new ViewCell { View = nameLabel };
        });

        listView.ItemTemplate = dataTemplate;

        Content = new StackLayout { Children = { listView } };
    }
}