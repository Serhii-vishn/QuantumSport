namespace QuantumSport.API.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public IList<IndividualMealPlanOrderEntity> IndividualMealPlanOrders { get; set; } = new List<IndividualMealPlanOrderEntity>();
        public IList<IndividualTrainingProgramOrderEntity> IndividualTrainingProgramOrders { get; set; } = new List<IndividualTrainingProgramOrderEntity>();
        public IList<UserTrainingRecordEntity> Trainings { get; set; } = new List<UserTrainingRecordEntity>();
    }
}
