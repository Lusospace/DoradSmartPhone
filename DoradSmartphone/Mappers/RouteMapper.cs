using AutoMapper;
using DoradSmartphone.DTO;
using DoradSmartphone.Models;

namespace DoradSmartphone.Mappers
{
    public class RouteMapper : Profile
    {
        public RouteMapper()
        {
            CreateMap<Route, RoutesDTO>();
            CreateMap<Widget, WidgetDTO>();
        }
    }
}
