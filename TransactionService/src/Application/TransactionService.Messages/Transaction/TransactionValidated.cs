namespace AntifraudService.Messages.Transaction;
public record TransactionValidated(Guid TransferId,
    TransactionService.Domain.Transaction.Enums.TransactionStatus TransactionStatus);
