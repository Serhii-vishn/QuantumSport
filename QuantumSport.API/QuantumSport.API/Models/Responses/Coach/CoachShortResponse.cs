namespace QuantumSport.API.Models.Responses
{
    public class CoachShortResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public IList<SportSectionNavigationResponse> SportSections { get; set; } = new List<SportSectionNavigationResponse>();
    }
}
