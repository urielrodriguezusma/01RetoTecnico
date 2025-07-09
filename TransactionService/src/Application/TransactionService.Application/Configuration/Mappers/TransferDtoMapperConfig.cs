using AutoMapper;
using TransactionService.Application.Transactions.Models;
using TransactionService.Domain.Transaction.Entities;

namespace TransactionService.Application.Configuration.Mappers;
public class TransferDtoMapperConfig : Profile
{
    public TransferDtoMapperConfig()
    {
        CreateMap<Transaction, TransactionDto>();
    }
}
