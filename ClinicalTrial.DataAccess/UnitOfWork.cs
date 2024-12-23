namespace ClinicalTrial.DataAccess
{
    internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
         => _dbContext.SaveChangesAsync(cancellationToken);
    }
}
