namespace QuantumSport.API.Mapping.Resolvers
{
    public class SportSectionPictureResolver : IValueResolver<SportSectionEntity, SportSectionDTO, string>
    {
        private readonly AppConfig _config;

        public SportSectionPictureResolver(IOptionsSnapshot<AppConfig> config)
        {
            _config = config.Value;
        }

        public string Resolve(SportSectionEntity source, SportSectionDTO destination, string destMember, ResolutionContext context)
        {
            if (source.PictureFileName == null)
            {
                return $"{_config.ApiHost}/{_config.ImgUrl}/no-image.jpg";
            }

            return $"{_config.ApiHost}/{_config.ImgUrl}/sport-sections/{source.PictureFileName}";
        }
    }
}
