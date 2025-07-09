using MediatR;
using Microsoft.Extensions.Logging;
using TransactionService.Application.UnitOfWork;
using TransactionService.Domain.Transaction.Repositories;

namespace TransactionService.Application.Transactions.Commands.UpdateTransactionStatus;
public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransacionStatusCommand>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTransactionStatusCommandHandler> _logger;

    public UpdateTransactionStatusCommandHandler(ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateTransactionStatusCommandHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task Handle(UpdateTransacionStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating transaction with transfer id {transferId} with status {status}.", request.TransferId, request.TransactionStatus);
        var transaction = await _transactionRepository.GetByIdAsync(request.TransferId, cancellationToken);
        if (transaction is null)
        {
            _logger.LogWarning("Transaction with transfer id {transferId} not found.", request.TransferId);
            return;
        }
        transaction?.UpdateStatus(request.TransactionStatus);
        await _unitOfWork.SaveChangesAsync();
        _logger.LogInformation("Transaction with transfer id {transferId} updated to status {status}.", request.TransferId, transaction!.Status);
    }
}
