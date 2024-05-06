using AutoMapper;
using ptdn_net.Data.Dto.Customer;
using ptdn_net.Data.Entity.CustomerEntity;

namespace ptdn_net.Mapping;

public class CustomerGroupProfile : Profile
{
    public CustomerGroupProfile()
    {
        CreateMap<CustomerGroup, CustomerGroupReq>().ReverseMap();
        CreateMap<CustomerGroup, CustomerGroupResp>().ReverseMap();
    }
}