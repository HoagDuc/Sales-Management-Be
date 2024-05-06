using AutoMapper;
using ptdn_net.Data.Dto.Transaction;
using ptdn_net.Data.Entity.TransactionEntity;

namespace ptdn_net.Mapping;

public class TransactionTypeProfile : Profile
{
    public TransactionTypeProfile()
    {
        CreateMap<TransactionType, TransactionTypeReq>().ReverseMap();
        CreateMap<TransactionType, TransactionTypeResp>().ReverseMap();
    }
}