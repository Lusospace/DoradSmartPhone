using DoradSmartphone.Services.Bluetooth;
using DoradSmartphone.ViewModels;

namespace DoradSmartphone.Views
{
    public partial class GlassPage : ContentPage
    {        
        public GlassPage()
        {
            InitializeComponent();            
            BindingContext = new GlassViewModel();
        }
    }
}
