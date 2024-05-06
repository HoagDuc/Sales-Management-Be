using AutoMapper;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleReq>().ReverseMap();
        CreateMap<Role, RoleResp>().ReverseMap();
    }
}