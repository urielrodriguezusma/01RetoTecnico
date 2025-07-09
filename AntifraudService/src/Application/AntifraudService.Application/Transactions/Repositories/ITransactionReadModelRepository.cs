namespace AntifraudService.Application.Transactions.Repositories;
public interface ITransactionReadModelRepository
{
    Task<decimal> GetTotalValueInCurrentDayByAccountId(Guid accountId, CancellationToken cancellationToken = default);
}
