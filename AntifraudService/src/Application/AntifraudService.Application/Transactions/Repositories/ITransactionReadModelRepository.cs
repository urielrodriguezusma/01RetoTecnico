using AntifraudService.Domain.Transaction.Entities;

namespace AntifraudService.Application.Transactions.Repositories;
public interface ITransactionReadModelRepository
{
    Task<decimal> GetTotalValueInCurrentDayByAccountId(Guid accountId, CancellationToken cancellationToken = default);

    Task<Transaction?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken = default);
}
