using Microsoft.EntityFrameworkCore.Storage;
using TransactionService.Application.UnitOfWork;

namespace TransactionService.Persistence.Sql;
public class UnitOfWork : IUnitOfWork
{
    private readonly TransactionDbContext _transactionDbContext;
    private IDbContextTransaction? _currentTransaction;

    public UnitOfWork(TransactionDbContext transactionDbContext)
    {
        _transactionDbContext = transactionDbContext;
    }
    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _currentTransaction = await _transactionDbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync()
    {
        await _transactionDbContext.SaveChangesAsync();
        if (_currentTransaction != null)
            await _currentTransaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        if (_currentTransaction != null)
            await _currentTransaction.RollbackAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _transactionDbContext.SaveChangesAsync();
    }
}
