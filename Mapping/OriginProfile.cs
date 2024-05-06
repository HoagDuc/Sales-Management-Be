using AutoMapper;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Mapping;

public class OriginProfile : Profile
{
    public OriginProfile()
    {
        CreateMap<Origin, OriginReq>().ReverseMap();
        CreateMap<Origin, OriginResp>().ReverseMap();
    }
}