
using TransactionService.Domain.Transaction.Enums;

namespace TransactionService.Domain.Transaction.Entities;
public class Transaction
{
    public Guid TransferId { get; private set; }
    public Guid SourceAccountId { get; private set; }
    public Guid TargetAccountId { get; private set; }
    public decimal Value { get; private set; }
    public DateTimeOffset CreateAt { get; private set; }
    public TransactionStatus Status { get; private set; }
    private Transaction()
    {
        TransferId = Guid.NewGuid();
        CreateAt = DateTime.UtcNow;
        Status = TransactionStatus.Pending;
    }

    public static Transaction Create(Guid sourceAccountId, Guid targetAccountId, decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Value must be greater than zero.", nameof(value));

        return new Transaction
        {
            SourceAccountId = sourceAccountId,
            TargetAccountId = targetAccountId,
            Value = value
        };
    }

    public void UpdateStatus(TransactionStatus status)
    {
        Status = status;
    }
}
