namespace TransactionService.Application.Transactions.Models;
public class CreateTransactionRequest
{
    public Guid SourceAccountId { get; init; }
    public Guid TargetAccountId { get; init; }
    public decimal Value { get; init; }
}
