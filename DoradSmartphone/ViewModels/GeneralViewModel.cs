using DoradSmartphone.DTO;
using static Android.Icu.Text.CaseMap;

namespace DoradSmartphone.ViewModels
{
    public class GeneralViewModel : BaseViewModel
    {
        private GlassDTO glassDTO;
        public GlassDTO GlassDTO
        {
            get => glassDTO;
            set => SetProperty(ref glassDTO, value);
        }

        public GeneralViewModel(GlassDTO glassDTO)
        {
            Title = "Review Page";
            GlassDTO = glassDTO;
        }
    }
}
