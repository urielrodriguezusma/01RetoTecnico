using AntifraudService.Domain.Transaction.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AntifraudService.Persistence.Sql.Transactions.Configurations;
public class TransactionsConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.TransferId);
        builder.Property(t => t.TransferId).ValueGeneratedNever();
    }
}
