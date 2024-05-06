using ptdn_net.Data.Dto.User;
using ptdn_net.Data.Entity.SystemEntity;

namespace ptdn_net.Services.System.Interfaces;

public interface IUserService
{
    Task<List<UserResp>> GetAllUsers();

    Task<UserResp> GetById(long id);

    Task<long> Create(UserReq req);

    Task<UserResp> Update(long id, UserReq req);

    Task<User> GetByUsername(string username);

    Task<long> ResetPassword(ResetPass req);

    Task<long> Delete(long id);
    
    Task<User?> GetByEmail(string email);
}