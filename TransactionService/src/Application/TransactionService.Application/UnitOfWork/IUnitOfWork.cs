namespace TransactionService.Application.UnitOfWork;
public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task SaveChangesAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
