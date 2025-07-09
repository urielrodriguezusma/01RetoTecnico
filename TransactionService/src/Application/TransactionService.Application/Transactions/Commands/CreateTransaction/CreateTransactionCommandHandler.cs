using MassTransit;
using MediatR;
using TransactionService.Application.Transactions.Models;
using TransactionService.Application.UnitOfWork;
using TransactionService.Domain.Transaction.Entities;
using TransactionService.Domain.Transaction.Repositories;
using TransactionService.Messages.Transaction;

namespace TransactionService.Application.Transactions.Commands.CreateTransaction;
public sealed class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository,
        IPublishEndpoint publishEndpoint,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _publishEndpoint = publishEndpoint;
        _unitOfWork = unitOfWork;
    }
    public async Task<CreateTransactionResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var newTransaction = Transaction.Create(request.Transaction.SourceAccountId,
            request.Transaction.TargetAccountId,
            request.Transaction.Value);

        await _transactionRepository.AddAsync(newTransaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        await _publishEndpoint.Publish<TransactionCreated>(new()
        {
            TransferId = newTransaction.TransferId,
            AccountId = newTransaction.SourceAccountId,
        }, cancellationToken);
        return new CreateTransactionResponse { TransferId = newTransaction.TransferId };
    }
}
