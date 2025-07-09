using MediatR;

namespace AntifraudService.Application.Transactions.Queries.GetTransactionById;
public record GetTransactionByIdQuery(Guid TransferId, Guid AccountId) : IRequest;
