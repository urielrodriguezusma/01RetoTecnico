using AntifraudService.Application.Transactions.Queries.GetTransactionById;
using AntifraudService.Application.Transactions.Repositories;
using AntifraudService.Domain;
using AntifraudService.Domain.Transaction.Entities;
using AntifraudService.Messages.Transaction;
using MassTransit;
using NSubstitute;

namespace AntifraudService.Application.UnitTests.Transactions.Queries.GetTransactionById;
public class GetTransactionByIdQueryHandlerTests
{
    private readonly ITransactionReadModelRepository _transactionReadModelRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public GetTransactionByIdQueryHandlerTests()
    {
        _transactionReadModelRepository = Substitute.For<ITransactionReadModelRepository>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
    }

    [Fact]
    public async Task Handler_When_TransactionsPerDayAreLessThanLimit_ShouldPublishStatusToApproved()
    {
        // Arrange
        var query = new GetTransactionByIdQuery(Guid.NewGuid(), Guid.NewGuid());
        _transactionReadModelRepository.GetTotalValueInCurrentDayByAccountId(query.AccountId, CancellationToken.None)
            .Returns(Task.FromResult(Globals.LimitPerDay - 1));

        var handler = new GetTransactionByIdQueryHandler(_transactionReadModelRepository, _publishEndpoint);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        await _publishEndpoint.Received(1).Publish(
              Arg.Is<TransactionValidated>(x => x.TransferId == query.TransferId && x.TransactionStatus == Domain.Transaction.Enums.TransactionStatus.Approved),
              CancellationToken.None);
    }

    [Fact]
    public async Task Handler_When_CurrentTransactionIsGreaterThanLimit_ShouldPublishStatusToRejected()
    {
        // Arrange
        var transaction = Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), Globals.LimitPerDay + 1);
        var query = new GetTransactionByIdQuery(transaction.TransferId, transaction.SourceAccountId);

        _transactionReadModelRepository.GetByIdAsync(query.TransferId, CancellationToken.None)
             .Returns(transaction);

        var handler = new GetTransactionByIdQueryHandler(_transactionReadModelRepository, _publishEndpoint);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        await _publishEndpoint.Received(1).Publish(
              Arg.Is<TransactionValidated>(x => x.TransferId == query.TransferId && x.TransactionStatus == Domain.Transaction.Enums.TransactionStatus.Rejected),
              CancellationToken.None);
    }


    [Fact]
    public async Task Handler_When_TransactionsPerDayAreGreaterThanLimit_ShouldPublishStatusToRejected()
    {
        // Arrange
        var transaction = Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), Globals.LimitPerDay);
        var query = new GetTransactionByIdQuery(transaction.TransferId, transaction.SourceAccountId);

        _transactionReadModelRepository.GetByIdAsync(query.TransferId, CancellationToken.None)
             .Returns(transaction);

        _transactionReadModelRepository.GetTotalValueInCurrentDayByAccountId(query.AccountId, CancellationToken.None)
            .Returns(Task.FromResult(Globals.LimitPerDay + 1));

        var handler = new GetTransactionByIdQueryHandler(_transactionReadModelRepository, _publishEndpoint);

        // Act
        await handler.Handle(query, CancellationToken.None);

        // Assert
        await _publishEndpoint.Received(1).Publish(
              Arg.Is<TransactionValidated>(x => x.TransferId == query.TransferId && x.TransactionStatus == Domain.Transaction.Enums.TransactionStatus.Rejected),
              CancellationToken.None);
    }
}
