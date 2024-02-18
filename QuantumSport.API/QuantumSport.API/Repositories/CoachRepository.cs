namespace QuantumSport.API.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        private readonly ApplicationDbContext _context;

        public CoachRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CoachEntity?> GetAsync(int id)
        {
            var coach = await _context.Coaches
                .Where(c => c.Id == id)
                .Include(c => c.SportSections)
                .ThenInclude(s => s.SportSection)
                .SingleOrDefaultAsync();
            return coach;
        }

        public async Task<IList<CoachEntity>> ListAsync()
        {
            return await _context.Coaches
                .Include(c => c.SportSections)
                .ThenInclude(s => s.SportSection)
                .ToListAsync();
        }
    }
}
