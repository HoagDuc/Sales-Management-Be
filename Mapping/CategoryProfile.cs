using AutoMapper;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Mapping;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryResp>().ReverseMap();
        CreateMap<Category, CategoryReq>().ReverseMap();
    }
}