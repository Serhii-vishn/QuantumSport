namespace QuantumSport.API.Models.DTOs
{
    public class SportSectionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
    }
}
