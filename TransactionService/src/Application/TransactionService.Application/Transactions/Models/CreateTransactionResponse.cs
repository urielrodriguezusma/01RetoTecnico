namespace TransactionService.Application.Transactions.Models;
public class CreateTransactionResponse
{
    public Guid TransferExternalId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
