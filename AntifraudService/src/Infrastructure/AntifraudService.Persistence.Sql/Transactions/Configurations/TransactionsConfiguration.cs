using AntifraudService.Domain.Transaction.Entities;
using AntifraudService.Domain.Transaction.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AntifraudService.Persistence.Sql.Transactions.Configurations;
public class TransactionsConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.TransferId);
        builder.Property(t => t.TransferId).ValueGeneratedNever();

        var statusConverter = new ValueConverter<TransactionStatus, string>(
            v => v.ToString(),
            v => (TransactionStatus)Enum.Parse(typeof(TransactionStatus), v));

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion(statusConverter);
    }
}
