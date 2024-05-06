using AutoMapper;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Mapping;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandReq>().ReverseMap();
        CreateMap<Brand, BrandResp>().ReverseMap();
    }
}