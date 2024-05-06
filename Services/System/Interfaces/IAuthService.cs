using Microsoft.AspNetCore.Mvc;
using ptdn_net.Data.Dto.Auth;

namespace ptdn_net.Services.System.Interfaces;

public interface IAuthService
{
    Task<LoginRes> Login(LoginReq req);

    Task<string> CreateToken(string username);
   
    Task ForgotPasswordAsync(string email);
}