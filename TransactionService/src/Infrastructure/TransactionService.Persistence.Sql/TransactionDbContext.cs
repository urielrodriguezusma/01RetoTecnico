using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Transaction.Entities;

namespace TransactionService.Persistence.Sql;
public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
