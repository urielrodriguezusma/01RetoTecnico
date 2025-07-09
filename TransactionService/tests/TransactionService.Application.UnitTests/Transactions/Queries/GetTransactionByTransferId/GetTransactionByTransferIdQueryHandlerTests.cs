using AutoMapper;
using TransactionService.Application.Transactions.Models;
using TransactionService.Application.Transactions.Queries.GetTransactionByTransferId;
using TransactionService.Application.Utils.Exceptions;
using TransactionService.Domain.Transaction.Entities;
using TransactionService.Domain.Transaction.Repositories;

namespace TransactionService.Application.UnitTests.Transactions.Queries.GetTransactionByTransferId;
public class GetTransactionByTransferIdQueryHandlerTests
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    public GetTransactionByTransferIdQueryHandlerTests()
    {
        _transactionRepository = Substitute.For<ITransactionRepository>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async Task Handler_WhenTransactionExists_ShouldReturnTransactionDto()
    {
        // Arrange
        var transaction = Transaction.Create(Guid.NewGuid(), Guid.NewGuid(), 100.00m);

        _transactionRepository.GetByIdAsync(Arg.Is<Guid>(d => d == transaction.TransferId), CancellationToken.None)
            .Returns(transaction);

        _mapper.Map<TransactionDto>(Arg.Any<Transaction>())
            .Returns(new TransactionDto
            {
                TransferId = transaction.TransferId,
                SourceAccountId = transaction.SourceAccountId,
                TargetAccountId = transaction.TargetAccountId,
                Value = transaction.Value,
                Status = transaction.Status
            });

        var handler = new GetTransactionByTransferIdQueryHandler(_transactionRepository, _mapper);

        // Act
        var result = await handler.Handle(new GetTransactionByTransferIdQuery(transaction.TransferId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull().And.BeOfType<TransactionDto>();
        result.TransferId.Should().Be(transaction.TransferId);
        result.SourceAccountId.Should().Be(transaction.SourceAccountId);
        result.TargetAccountId.Should().Be(transaction.TargetAccountId);
        result.Value.Should().Be(transaction.Value);
    }

    [Fact]
    public async Task Handler_WhenTransactionDoesNotExists_ShouldThrowNotFoundException()
    {
        // Arrange
        var transferId = Guid.NewGuid();
        _transactionRepository.GetByIdAsync(Arg.Any<Guid>(), CancellationToken.None)
            .Returns(Task.FromResult<Transaction?>(null));

        var handler = new GetTransactionByTransferIdQueryHandler(_transactionRepository, _mapper);

        // Act
        var exception = await Record.ExceptionAsync(() => handler.Handle(new GetTransactionByTransferIdQuery(transferId), CancellationToken.None));

        // Assert
        exception.Should().NotBeNull().And.BeOfType<NotFoundException>()
            .Which.Message.Should().Be($"Transaction with id {transferId} was not found.");
    }
}
