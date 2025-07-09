using TransactionService.Domain.Transaction.Enums;

namespace TransactionService.Application.Transactions.Models;
public class TransactionDto
{
    public Guid TransferId { get; init; }
    public Guid SourceAccountId { get; init; }
    public Guid TargetAccountId { get; init; }
    public decimal Value { get; init; }
    public TransactionStatus Status { get; init; }
}
