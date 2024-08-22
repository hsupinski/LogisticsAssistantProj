using LogisticsAssistantProject.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LogisticsAssistantProject.Data
{
    public class LoggerDbContext : DbContext
    {
        public LoggerDbContext(DbContextOptions<LoggerDbContext> options) : base(options)
        {
            
        }

        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LogEntry>().ToTable("LogEntries");
        }
    }
}
