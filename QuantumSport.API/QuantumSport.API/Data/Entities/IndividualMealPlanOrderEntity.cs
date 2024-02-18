namespace QuantumSport.API.Data.Entities
{
    public class IndividualMealPlanOrderEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; } = null!;

        public int NutritionistId { get; set; }
        public NutritionistEntity Nutritionist { get; set; } = null!;
    }
}
