using AutoMapper;
using DoradSmartphone.DTO;
using DoradSmartphone.Mappers;
using DoradSmartphone.Models;

namespace DoradSmartphone.Helpers
{
    public static class EntityToDto
    {
        public static IMapper Mapper { get; }

        static EntityToDto()
        {
            var config = new MapperConfiguration(cfg =>
            {                
                cfg.AddProfile<RouteMapper>();
                cfg.CreateMap<TransferDTO, GlassDTO>()
                    .ForMember(dest => dest.WidgetDTOs, opt => opt.MapFrom(src => src.Widgets))
                    .ForMember(dest => dest.RoutesDTOs, opt => opt.MapFrom(src => src.Route));
            });

            Mapper = config.CreateMapper();
        }

        public static GlassDTO Convertion(TransferDTO transferDTO)
        {
            // Filter out widgets with IsInvisible set to true
            List<Widget> visibleWidgets = transferDTO.Widgets
                .Where(widget => !widget.IsInvisible)
                .ToList();

            // Create a new TransferDTO with filtered widgets
            TransferDTO filteredTransferDTO = new TransferDTO
            {
                Exercise = transferDTO.Exercise,
                Widgets = visibleWidgets,
                Route = transferDTO.Route,
                Avatar = transferDTO.Avatar
            };

            // Use the IMapper instance to map TransferDTO to GlassDTO
            return Mapper.Map<GlassDTO>(filteredTransferDTO);
        }
    }
}
