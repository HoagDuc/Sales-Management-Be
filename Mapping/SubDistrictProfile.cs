using AutoMapper;
using ptdn_net.Data.Dto.Location;
using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Mapping;

public class SubDistrictProfile : Profile
{
    public SubDistrictProfile()
    {
        CreateMap<Subdistrict, SubDistrictResp>().ReverseMap();
    }
}