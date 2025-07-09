using MediatR;
using TransactionService.Domain.Transaction.Enums;

namespace TransactionService.Application.Transactions.Commands.UpdateTransactionStatus;
public record class UpdateTransacionStatusCommand(Guid TransferId, TransactionStatus TransactionStatus) : IRequest;
