namespace DoradSmartphone.DTO
{
    public class GlassDTO
    { 
        public AvatarDTO Avatar { get; set; }
        public List<RoutesDTO> RoutesDTOs { get; set; }
        public List<WidgetDTO> WidgetDTOs { get; set; }
        public bool WidgetConfiguration { get; set; }
    }
}
