namespace QuantumSport.API.Models.Responses
{
    public class CoachModelResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Education { get; set; } = null!;
        public string Achievement { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public IList<SportSectionNavigationResponse> SportSections { get; set; } = new List<SportSectionNavigationResponse>();
    }
}
