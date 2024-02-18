namespace QuantumSport.API.Data.Entities
{
    public class NutritionistEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Education { get; set; } = null!;
        public string Spezialization { get; set; } = null!;
        public string? PictureFileName { get; set; }

        public IList<IndividualMealPlanOrderEntity> IndividualMealPlanOrders { get; set; } = new List<IndividualMealPlanOrderEntity>();
    }
}
