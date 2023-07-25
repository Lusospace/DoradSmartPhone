using DoradSmartphone.DTO;

namespace DoradSmartphone.ViewModels
{
    public class GeneralViewModel : BaseViewModel
    {
        private TransferDTO transferDTO;
        public TransferDTO TransferDTO
        {
            get => transferDTO;
            set => SetProperty(ref transferDTO, value);
        }

        public GeneralViewModel(TransferDTO transferDTO)
        {
            Title = "Review Page";
            TransferDTO = transferDTO;
        }
    }
}
