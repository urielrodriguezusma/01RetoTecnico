using AntifraudService.Application.Transactions.Queries.GetTransactionById;
using MassTransit;
using MediatR;
using TransactionService.Messages.Transaction;

namespace AntifraudService.Worker.Transactions.Consumers;
public class OnTransactionCreatedThenValidateStatus : IConsumer<TransactionCreated>
{
    private readonly ISender _mediator;
    private readonly ILogger<OnTransactionCreatedThenValidateStatus> _logger;

    public OnTransactionCreatedThenValidateStatus(ISender mediator,
        ILogger<OnTransactionCreatedThenValidateStatus> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<TransactionCreated> context)
    {
        var message = context.Message;
        _logger.LogInformation("Consuming {consumer}. Message:{message}", nameof(TransactionCreated), message);
        await _mediator.Send(new GetTransactionByIdQuery(message.TransferId, message.AccountId), context.CancellationToken);
    }
}

