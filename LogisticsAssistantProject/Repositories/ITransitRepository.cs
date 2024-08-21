using LogisticsAssistantProject.Models.Domain;

namespace LogisticsAssistantProject.Repositories
{
    public interface ITransitRepository
    {
        Task<Transit> AddAsync(Transit transit);
        Task<IEnumerable<Transit>> GetByTruckIdAsync(int id);
    }
}