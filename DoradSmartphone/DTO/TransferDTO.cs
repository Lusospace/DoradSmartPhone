using DoradSmartphone.Models;

namespace DoradSmartphone.DTO
{
    public class TransferDTO
    {
        public Exercise Exercise { get; set; }
        public List<Widget> Widgets { get; set; }
        public List<Route> Route { get; set; }
        public AvatarDTO Avatar { get; set; }
    }
}
