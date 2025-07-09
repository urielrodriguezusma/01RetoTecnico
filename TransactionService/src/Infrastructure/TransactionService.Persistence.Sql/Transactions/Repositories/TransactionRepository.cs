using TransactionService.Domain.Transaction.Entities;
using TransactionService.Domain.Transaction.Repositories;
using TransactionService.Persistence.Sql;

namespace AntifraudService.Persistence.Sql.Transactions.Repositories;
public class TransactionRepository : ITransactionRepository
{
    private readonly TransactionDbContext _dbContext;

    public TransactionRepository(TransactionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken = default)
    {
        await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
    }

    public async Task<Transaction?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Transactions.FindAsync([Id, cancellationToken], cancellationToken: cancellationToken);
    }

    public void Update(Transaction transaction)
    {
        _dbContext.Update(transaction);
    }
}
