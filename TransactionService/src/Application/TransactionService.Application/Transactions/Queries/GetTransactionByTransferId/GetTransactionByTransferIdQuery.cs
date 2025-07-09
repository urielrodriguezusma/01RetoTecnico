using MediatR;
using TransactionService.Application.Transactions.Models;

namespace TransactionService.Application.Transactions.Queries.GetTransactionByTransferId;
public record GetTransactionByTransferIdQuery(Guid TransferId) : IRequest<TransactionDto>;
