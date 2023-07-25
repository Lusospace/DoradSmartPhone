using AutoMapper;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;

namespace DoradSmartphone.Mappers
{
    public class WidgetMapper : Profile
    {
        public WidgetMapper()
        {
            CreateMap<Widget, WidgetDTO>();
        }
    }
}
