namespace QuantumSport.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserDTO>().ReverseMap();
        }
    }
}
