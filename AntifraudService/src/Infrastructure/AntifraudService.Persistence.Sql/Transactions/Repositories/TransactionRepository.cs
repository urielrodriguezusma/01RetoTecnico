using AntifraudService.Application.Transactions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AntifraudService.Persistence.Sql.Transactions.Repositories;
public class TransactionRepository : ITransactionReadModelRepository
{
    private readonly AntifraudDbContext _dbContext;

    public TransactionRepository(AntifraudDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<decimal> GetTotalValueInCurrentDayByAccountId(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Transactions
             .Where(t => t.SourceAccountId == accountId && t.CreateAt.Date == DateTime.UtcNow.Date)
             .SumAsync(t => t.Value, cancellationToken);
    }
}
