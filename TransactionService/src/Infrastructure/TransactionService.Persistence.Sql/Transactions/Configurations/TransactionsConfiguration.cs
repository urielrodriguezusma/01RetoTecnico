using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TransactionService.Domain;
using TransactionService.Domain.Transaction.Entities;
using TransactionService.Domain.Transaction.Enums;

namespace TransactionService.Persistence.Sql.Transactions.Configurations;
public class TransactionsConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.TransferId);
        builder.Property(t => t.TransferId).ValueGeneratedNever();

        builder.Property(t => t.SourceAccountId)
            .IsRequired();

        builder.Property(t => t.TargetAccountId)
            .IsRequired();

        builder.Property(t => t.Value)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.CreateAt)
            .IsRequired();

        var statusConverter = new ValueConverter<TransactionStatus, string>(
            v => v.ToString(),
            v => (TransactionStatus)Enum.Parse(typeof(TransactionStatus), v));

        builder.Property(t => t.Status)
            .IsRequired()
            .HasMaxLength(Globals.StatusMaxLength)
            .HasConversion(statusConverter);
    }
}
