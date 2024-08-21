using LogisticsAssistantProject.Data;
using LogisticsAssistantProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistantProject.Repositories
{
    public class TransitRepository : ITransitRepository
    {
        private readonly AssistantDbContext _assistantDbContext;
        public TransitRepository(AssistantDbContext assistantDbContext)
        {
            _assistantDbContext = assistantDbContext;
        }

        public async Task<Transit> AddAsync(Transit transit)
        {
            await _assistantDbContext.AddAsync(transit);
            await _assistantDbContext.SaveChangesAsync();
            return transit;
        }

        public async Task<IEnumerable<Transit>> GetByTruckIdAsync(int id)
        {
            return await _assistantDbContext.Transit.Where(t => t.TruckId == id).ToListAsync();
        }
    }
}
