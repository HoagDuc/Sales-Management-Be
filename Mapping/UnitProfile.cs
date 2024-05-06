using AutoMapper;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Mapping;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<Unit, UnitReq>().ReverseMap();
        CreateMap<Unit, UnitResp>().ReverseMap();
    }
}