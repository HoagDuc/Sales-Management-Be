using AutoMapper;
using ptdn_net.Data.Dto.Location;
using ptdn_net.Data.Entity.LocationEntity;

namespace ptdn_net.Mapping;

public class ProvinceProfile : Profile
{
    public ProvinceProfile()
    {
        CreateMap<Province, ProvinceResp>().ReverseMap();
    }
}