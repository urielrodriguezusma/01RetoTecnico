using AutoMapper;
using MediatR;
using TransactionService.Application.Transactions.Models;
using TransactionService.Application.Utils.Exceptions;
using TransactionService.Domain.Transaction.Repositories;

namespace TransactionService.Application.Transactions.Queries.GetTransactionByTransferId;
public class GetTransactionByTransferIdQueryHandler : IRequestHandler<GetTransactionByTransferIdQuery, TransactionDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public GetTransactionByTransferIdQueryHandler(ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }
    public async Task<TransactionDto> Handle(GetTransactionByTransferIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.TransferId, cancellationToken) ??
            throw new NotFoundException($"Transaction with id {request.TransferId} was not found.");

        return _mapper.Map<TransactionDto>(transaction);
    }
}
