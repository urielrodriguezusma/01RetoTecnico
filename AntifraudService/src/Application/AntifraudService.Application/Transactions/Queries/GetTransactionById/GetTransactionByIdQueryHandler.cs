using AntifraudService.Application.Transactions.Repositories;
using AntifraudService.Domain;
using AntifraudService.Domain.Transaction.Enums;
using AntifraudService.Messages.Transaction;
using MassTransit;
using MediatR;

namespace AntifraudService.Application.Transactions.Queries.GetTransactionById;
public sealed class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery>
{
    private readonly ITransactionReadModelRepository _transactionRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public GetTransactionByIdQueryHandler(ITransactionReadModelRepository transactionRepository,
        IPublishEndpoint publishEndpoint)
    {
        _transactionRepository = transactionRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        TransactionValidated transactionValidated;
        var currentTransaction = await _transactionRepository.GetByIdAsync(request.TransferId, cancellationToken);

        if (currentTransaction!.Value > Globals.LimitTransactionValue)
            transactionValidated = new(request.TransferId, TransactionStatus.Rejected);
        else
        {
            var totalTransactionsInCurrentDay = await _transactionRepository.GetTotalValueInCurrentDayByAccountId(request.AccountId, cancellationToken);
            if (totalTransactionsInCurrentDay <= Globals.LimitPerDay)
                transactionValidated = new(request.TransferId, TransactionStatus.Approved);
            else
                transactionValidated = new(request.TransferId, TransactionStatus.Rejected);
        }

        await _publishEndpoint.Publish(transactionValidated, cancellationToken);
    }
}
