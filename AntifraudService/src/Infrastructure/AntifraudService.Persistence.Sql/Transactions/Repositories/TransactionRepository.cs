using AntifraudService.Application.Transactions.Repositories;
using AntifraudService.Domain.Transaction.Entities;
using AntifraudService.Domain.Transaction.Enums;
using Microsoft.EntityFrameworkCore;

namespace AntifraudService.Persistence.Sql.Transactions.Repositories;
public class TransactionRepository : ITransactionReadModelRepository
{
    private readonly AntifraudDbContext _dbContext;

    public TransactionRepository(AntifraudDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Transaction?> GetByIdAsync(Guid transferId, CancellationToken cancellationToken = default)
    {
        var currentTransaction = await _dbContext.FindAsync<Transaction>(transferId, cancellationToken);
        return currentTransaction;
    }

    public async Task<decimal> GetTotalValueInCurrentDayByAccountId(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Transactions
             .Where(t => t.SourceAccountId == accountId && t.CreateAt.Date == DateTime.UtcNow.Date && t.Status != TransactionStatus.Rejected)
             .SumAsync(t => t.Value, cancellationToken);
    }
}
