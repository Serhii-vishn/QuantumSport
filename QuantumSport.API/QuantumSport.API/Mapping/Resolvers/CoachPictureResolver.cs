namespace QuantumSport.API.Mapping.Resolvers
{
    public class CoachPictureResolver : IValueResolver<CoachEntity, CoachDTO, string>
    {
        private readonly AppConfig _config;

        public CoachPictureResolver(IOptionsSnapshot<AppConfig> config)
        {
            _config = config.Value;
        }

        public string Resolve(CoachEntity source, CoachDTO destination, string destMember, ResolutionContext context)
        {
            if (source.PictureFileName == null)
            {
                return $"{_config.ApiHost}/{_config.ImgUrl}/no-image.jpg";
            }

            return $"{_config.ApiHost}/{_config.ImgUrl}/coaches/{source.PictureFileName}";
        }
    }
}
