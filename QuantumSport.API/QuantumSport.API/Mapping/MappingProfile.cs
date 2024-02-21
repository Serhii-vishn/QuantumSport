using QuantumSport.API.Mapping.Resolvers;

namespace QuantumSport.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserDTO>().ReverseMap();
            CreateMap<SportSectionEntity, SportSectionDTO>()
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom<SportSectionPictureResolver>());
            CreateMap<CoachEntity, CoachDTO>()
                .ForMember(d => d.PictureUrl, opt => opt.MapFrom<CoachPictureResolver>())
                .ForMember(d => d.SportSections, opt => opt.MapFrom(s => s.SportSections.Select(ss => ss.SportSection).ToList()));

            CreateMap<SportSectionDTO, SportSectionNavigationResponse>();
            CreateMap<CoachDTO, CoachNavigationResponse>().ForMember(d => d.Name, opt => opt.MapFrom(c => c.FirstName + " " + c.LastName));
            CreateMap<CoachDTO, CoachShortResponse>().ForMember(d => d.Name, opt => opt.MapFrom(c => c.FirstName + " " + c.LastName));
            CreateMap<CoachDTO, CoachModelResponse>().ForMember(d => d.Name, opt => opt.MapFrom(c => c.FirstName + " " + c.LastName));

            CreateMap<IndividualTrainingProgramOrderRequest, IndividualTrainingProgramOrderRequestDTO>();
        }
    }
}
