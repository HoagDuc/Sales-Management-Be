using AutoMapper;
using ptdn_net.Data.Dto.Product;
using ptdn_net.Data.Entity.ProductEntity;

namespace ptdn_net.Mapping;

public class VendorProfile : Profile
{
    public VendorProfile()
    {
        CreateMap<Vendor, VendorReq>().ReverseMap();
        CreateMap<Vendor, VendorResp>().ReverseMap();
    }
}