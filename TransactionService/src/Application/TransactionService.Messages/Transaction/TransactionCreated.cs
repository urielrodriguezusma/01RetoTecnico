namespace TransactionService.Messages.Transaction;

public class TransactionCreated
{
    public Guid TransferId { get; init; }
    public Guid AccountId { get; init; }
}
