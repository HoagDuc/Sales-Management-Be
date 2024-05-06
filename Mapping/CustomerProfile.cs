using AutoMapper;
using ptdn_net.Data.Dto.Customer;
using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Mapping;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<Customer, CustomerReq>().ReverseMap();
        CreateMap<Customer, CustomerResp>().ReverseMap();
    }
}