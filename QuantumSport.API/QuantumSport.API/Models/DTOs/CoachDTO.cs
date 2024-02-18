namespace QuantumSport.API.Models.DTOs
{
    public class CoachDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Education { get; set; } = null!;
        public string Achievement { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public IList<SportSectionDTO> SportSections { get; set; } = new List<SportSectionDTO>();
    }
}
