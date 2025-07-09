using AntifraudService.Domain.Transaction.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntifraudService.Persistence.Sql;
public class AntifraudDbContext : DbContext
{
    public AntifraudDbContext(DbContextOptions<AntifraudDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
    }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
