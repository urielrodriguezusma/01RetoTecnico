using FluentAssertions;
using MassTransit;
using TransactionService.Application.Transactions.Commands.CreateTransaction;
using TransactionService.Application.Transactions.Models;
using TransactionService.Application.UnitOfWork;
using TransactionService.Domain.Transaction.Entities;
using TransactionService.Domain.Transaction.Repositories;
using TransactionService.Messages.Transaction;

namespace TransactionService.Application.UnitTests.Transactions.Commands.CreateTransaction;
public class CreateTransactionCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTransactionCommandHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _publishEndpoint = Substitute.For<IPublishEndpoint>();
    }

    [Fact]
    public async Task Handler_WhenTransacionDataIsValid_ShouldCreateTransaction()
    {
        // Arrange
        var transactionRequest = new CreateTransactionRequest
        {
            SourceAccountId = Guid.NewGuid(),
            TargetAccountId = Guid.NewGuid(),
            Value = 100.00m
        };

        var command = new CreateTransactionCommand(transactionRequest);
        _transactionRepository.AddAsync(Arg.Any<Transaction>(), CancellationToken.None)
            .Returns(Task.CompletedTask);

        var handler = new CreateTransactionCommandHandler(_transactionRepository, _publishEndpoint, _unitOfWork);

        // Act
        var response = await handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull().And.BeOfType<CreateTransactionResponse>();
        response.TransferId.Should().NotBeEmpty();

        await _transactionRepository.Received(1).AddAsync(Arg.Is<Transaction>(t =>
            t.SourceAccountId == transactionRequest.SourceAccountId &&
            t.TargetAccountId == transactionRequest.TargetAccountId &&
            t.Value == transactionRequest.Value), CancellationToken.None);

        await _unitOfWork.Received(1).SaveChangesAsync();
        await _publishEndpoint.Received(1).Publish(Arg.Any<TransactionCreated>(), CancellationToken.None);
    }
}
