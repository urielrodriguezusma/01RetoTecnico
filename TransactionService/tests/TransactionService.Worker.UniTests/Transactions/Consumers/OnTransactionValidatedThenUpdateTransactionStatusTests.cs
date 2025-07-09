using AntifraudService.Messages.Transaction;
using AutoFixture;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TransactionService.Application.Transactions.Commands.UpdateTransactionStatus;
using TransactionService.Worker.Transactions.Consumers;

namespace TransactionService.Worker.UniTests.Transactions.Consumers;
public class OnTransactionValidatedThenUpdateTransactionStatusTests
{
    private readonly IFixture _fixture;
    private readonly ISender _mediatorMock;
    private readonly ILogger<OnTransactionValidatedThenUpdateTransactionStatus> _loggerMock;
    private readonly OnTransactionValidatedThenUpdateTransactionStatus _consumer;

    public OnTransactionValidatedThenUpdateTransactionStatusTests()
    {
        _fixture = new Fixture();
        _mediatorMock = Substitute.For<ISender>();
        _loggerMock = Substitute.For<ILogger<OnTransactionValidatedThenUpdateTransactionStatus>>();
        _consumer = new OnTransactionValidatedThenUpdateTransactionStatus(_mediatorMock, _loggerMock);
    }

    [Fact]
    public async Task Consumer_WhenTransactionIsValidated_Should_UpdateTransaction()
    {
        // Arrange
        var transactionValidated = _fixture.Create<TransactionValidated>();
        var context = Substitute.For<ConsumeContext<TransactionValidated>>();
        context.Message.Returns(transactionValidated);

        // Act
        await _consumer.Consume(context);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Any<UpdateTransacionStatusCommand>(), CancellationToken.None);
    }
}
