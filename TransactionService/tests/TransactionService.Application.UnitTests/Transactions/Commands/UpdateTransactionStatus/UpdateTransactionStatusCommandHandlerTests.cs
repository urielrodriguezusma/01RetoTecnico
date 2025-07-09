
using Microsoft.Extensions.Logging;
using TransactionService.Application.Transactions.Commands.UpdateTransactionStatus;
using TransactionService.Application.UnitOfWork;
using TransactionService.Domain.Transaction.Entities;
using TransactionService.Domain.Transaction.Repositories;

namespace TransactionService.Application.UnitTests.Transactions.Commands.UpdateTransactionStatus;
public class UpdateTransactionStatusCommandHandlerTests
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private ILogger<UpdateTransactionStatusCommandHandler> _logger;
    public UpdateTransactionStatusCommandHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ILogger<UpdateTransactionStatusCommandHandler>>();
    }

    [Fact]
    public async Task Handler_WhenTransactionExists_ShouldUpdateTransactionStatus()
    {
        // Arrange
        var transaction = Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), 100.00m);
        UpdateTransacionStatusCommand command = new(transaction.TransferId, Domain.Transaction.Enums.TransactionStatus.Approved);
        _transactionRepository.GetByIdAsync(Arg.Is<Guid>(d => d == command.TransferId), CancellationToken.None)
            .Returns(transaction);

        var handler = new UpdateTransactionStatusCommandHandler(_transactionRepository, _unitOfWork, _logger);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        transaction.Status.Should().Be(command.TransactionStatus);
        await _transactionRepository.Received(1).GetByIdAsync(Arg.Is<Guid>(d => d == command.TransferId), CancellationToken.None);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handler_WhenTransferDoesNotExists_ShouldWarning()
    {
        // Arrange
        UpdateTransacionStatusCommand command = new(Guid.NewGuid(), Domain.Transaction.Enums.TransactionStatus.Approved);
        _transactionRepository.GetByIdAsync(Arg.Is<Guid>(d => d == command.TransferId), CancellationToken.None)
            .Returns(Task.FromResult<Transaction?>(null));

        var handler = new UpdateTransactionStatusCommandHandler(_transactionRepository, _unitOfWork, _logger);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        await _transactionRepository.Received(1).GetByIdAsync(Arg.Is<Guid>(d => d == command.TransferId), CancellationToken.None);
        await _unitOfWork.DidNotReceive().SaveChangesAsync();
        _logger.ReceivedWithAnyArgs().Received(1).Log(
              LogLevel.Warning,
              Arg.Any<EventId>(),
              Arg.Is<object>(o => o.ToString().Contains($"Transaction with transfer id {command.TransferId} not found.")),
              Arg.Any<Exception>(),
              Arg.Any<Func<object, Exception, string>>());
    }
}
