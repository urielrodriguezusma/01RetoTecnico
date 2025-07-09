namespace TransactionService.Domain.Transaction.Repositories;
public interface ITransactionRepository
{
    Task<Entities.Transaction?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task AddAsync(Entities.Transaction transaction, CancellationToken cancellationToken = default);
    void Update(Entities.Transaction transaction);
}
