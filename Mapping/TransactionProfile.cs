using AutoMapper;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Mapping;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionReq>().ReverseMap();
        CreateMap<Transaction, TransactionResp>().ReverseMap();
    }
}