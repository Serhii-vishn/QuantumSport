namespace QuantumSport.API.Data.Entities
{
    public class CoachEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Education { get; set; } = null!;
        public string Achievement { get; set; } = null!;
        public string? PictureFileName { get; set; }

        public IList<IndividualTrainingProgramOrderEntity> IndividualTrainingProgramOrders { get; set; } = new List<IndividualTrainingProgramOrderEntity>();
        public IList<CoachSportSectionEntity> SportSections { get; set; } = new List<CoachSportSectionEntity>();
    }
}
