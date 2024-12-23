using ClinicalTrial.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrial.DataAccess
{
    public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ClinicalTrialEntity> ClinicalTrials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
