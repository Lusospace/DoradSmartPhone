using DoradSmartphone.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using static Android.Icu.Text.CaseMap;
using static Microsoft.Maui.LifecycleEvents.AndroidLifecycle;

namespace DoradSmartphone.ViewModels
{
    public partial class ManualViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<Widget> WidgetsToOrganize { get; set; }

        public Command<Widget> WidgetDraggedCommand { get; }
        public Command<Widget> WidgetDroppedCommand { get; }

        private Widget draggedWidget;

        private ObservableCollection<Widget> widgets;
        public ObservableCollection<Widget> Widgets
        {
            get => widgets;
            set => SetProperty(ref widgets, value);
        }

        private ContentPage automaticPage;
        public ContentPage AutomaticPage
        {
            get => automaticPage;
            set => SetProperty(ref automaticPage, value);
        }        

        public ManualViewModel(List<Widget> selectedItems)
        {
            Title = "Manual Configuration";
            Widgets = new ObservableCollection<Widget>(selectedItems);

            WidgetsToOrganize = new ObservableCollection<Widget>(widgets);
            WidgetDraggedCommand = new Command<Widget>(OnWidgetDragged);
            WidgetDroppedCommand = new Command<Widget>(OnWidgetDropped);
        }

        private void OnWidgetDragged(Widget widget)
        {
            draggedWidget = widget;
        }

        private void OnWidgetDropped(Widget widget)
        {
            if (draggedWidget != null)
            {
                // Perform any necessary actions when the drop occurs
                // Update the position properties of the draggedWidget (PosX, PosY)

                draggedWidget = null;
            }
        }
    }
}
