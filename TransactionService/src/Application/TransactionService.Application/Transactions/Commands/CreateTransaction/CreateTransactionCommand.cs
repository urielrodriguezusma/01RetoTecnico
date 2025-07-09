using MediatR;
using TransactionService.Application.Transactions.Models;

namespace TransactionService.Application.Transactions.Commands.CreateTransaction;
public record CreateTransactionCommand(CreateTransactionRequest Transaction) : IRequest<CreateTransactionResponse>;

