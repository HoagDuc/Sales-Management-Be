using AutoMapper;
using ptdn_net.Data.Dto.User;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResp>().ReverseMap();
        CreateMap<User, UserReq>().ReverseMap();
    }
}