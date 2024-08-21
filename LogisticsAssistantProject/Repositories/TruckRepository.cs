using LogisticsAssistantProject.Data;
using LogisticsAssistantProject.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistantProject.Repositories
{
    public class TruckRepository : ITruckRepository
    {
        private readonly AssistantDbContext _assistantDbContext;

        public TruckRepository(AssistantDbContext assistantDbContext)
        {
            _assistantDbContext = assistantDbContext;
        }

        public async Task<Truck> AddAsync(Truck truck)
        {
            await _assistantDbContext.AddAsync(truck);
            await _assistantDbContext.SaveChangesAsync();
            return truck;
        }

        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _assistantDbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM Truck WHERE Id = {id}");
            return new OkResult();
        }

        public async Task<IEnumerable<Truck>> GetAllAsync()
        {
            return await _assistantDbContext.Truck.ToListAsync();
        }

        public async Task<Truck> GetByIdAsync(int id)
        {
            return await _assistantDbContext.Truck.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Truck> UpdateAsync(Truck truck)
        {
            _assistantDbContext.Update(truck);
            await _assistantDbContext.SaveChangesAsync();
            return truck;

        }
    }
}
