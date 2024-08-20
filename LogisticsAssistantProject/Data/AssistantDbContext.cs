using LogisticsAssistantProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistantProject.Data
{
    public class AssistantDbContext : DbContext
    {
        public AssistantDbContext(DbContextOptions<AssistantDbContext> options) : base(options)
        {
            
        }

        public DbSet<Transit> Transit { get; set; }
        public DbSet<Truck> Truck { get; set; }
        public DbSet<TruckInTransit> TruckInTransit { get; set; }
    }
}
