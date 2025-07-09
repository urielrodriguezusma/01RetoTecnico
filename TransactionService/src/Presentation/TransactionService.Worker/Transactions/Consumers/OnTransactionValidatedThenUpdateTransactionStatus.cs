using AntifraudService.Messages.Transaction;
using MassTransit;
using MediatR;
using TransactionService.Application.Transactions.Commands.UpdateTransactionStatus;

namespace TransactionService.Worker.Transactions.Consumers;
public class OnTransactionValidatedThenUpdateTransactionStatus : IConsumer<TransactionValidated>
{
    private readonly ISender _sender;
    private readonly ILogger<OnTransactionValidatedThenUpdateTransactionStatus> _logger;

    public OnTransactionValidatedThenUpdateTransactionStatus(ISender sender,
        ILogger<OnTransactionValidatedThenUpdateTransactionStatus> logger)
    {
        _sender = sender;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<TransactionValidated> context)
    {
        var message = context.Message;
        _logger.LogInformation("Consuming {consumer}, message: {message}", nameof(OnTransactionValidatedThenUpdateTransactionStatus), message);
        await _sender.Send(new UpdateTransacionStatusCommand(message.TransferId, message.TransactionStatus), context.CancellationToken);
        _logger.LogInformation("Message consumed {consumer}", message);
    }
}
