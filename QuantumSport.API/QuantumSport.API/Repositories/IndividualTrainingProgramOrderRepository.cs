namespace QuantumSport.API.Repositories
{
    public class IndividualTrainingProgramOrderRepository : IIndividualTrainingProgramOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public IndividualTrainingProgramOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(IndividualTrainingProgramOrderEntity trainingProgramOrder)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await _context.IndividualTrainingProgramOrders.AddAsync(trainingProgramOrder);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result.Entity.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
