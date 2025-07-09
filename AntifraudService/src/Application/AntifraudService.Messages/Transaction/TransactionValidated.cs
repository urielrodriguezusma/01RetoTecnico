namespace AntifraudService.Messages.Transaction;
public record TransactionValidated(Guid TransferId, Domain.Transaction.Enums.TransactionStatus TransactionStatus);
