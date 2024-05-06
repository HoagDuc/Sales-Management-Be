using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BT_MVC_Web.Exceptions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ptdn_net.Configuration.Models;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services.interfaces;
using ptdn_net.Services.System.Interfaces;
using ptdn_net.Utils.Email;

namespace ptdn_net.Services.System;

public class AuthService : BaseService, IAuthService
{
    private readonly JwtConfig _jwtConfig;
    private readonly IMemoryCache _memoryCache;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;


    public AuthService(
        IUnitOfWork unitOfWork,
        IUserService userService,
        IOptions<JwtConfig> jwtConfig,
        IMemoryCache memoryCache,
        IEmailService emailService) : base(unitOfWork)
    {
        _userService = userService;
        _memoryCache = memoryCache;
        _emailService = emailService;
        _jwtConfig = jwtConfig.Value;
    }


    public async Task<LoginRes> Login(LoginReq req)
    {
        var user = await _userService.GetByUsername(req.Username!);
        if (!BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            throw new BadRequestException("Username or password is incorrect");
        if (!user.IsActive) throw new BadRequestException("User is not active");

        var cachedUser = _memoryCache.Get<User>(user.Username);

        if (cachedUser is null)
        { 
            _memoryCache.Set(req.Username!, user, TimeSpan.FromHours(1));
        }
        return new LoginRes(user);
    }

    public async Task<string> CreateToken(string username)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, username),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var singeKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey!));
        var signingCredentials = new SigningCredentials(singeKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _jwtConfig.Issuer,
            _jwtConfig.Audience,
            claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: signingCredentials
        );

        var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
        return await Task.FromResult(tokenResult);
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userService.GetByEmail(email);
        if (user == null) throw new NotFoundException("Email invalid!!!");

        var random = new Random();
        string password = GenerateRandomPassword(random);

        user.Password = BCrypt.Net.BCrypt.HashPassword(password);
        UnitOfWork.UserRepo.Update(user);
        await UnitOfWork.CompleteAsync();
    
        await SendForgotPasswordEmail(email, password);
    }
    

    private static string GenerateRandomPassword(Random random)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    private async Task SendForgotPasswordEmail(string email, string password)
    {
        var mailRequest = new MailRequest
        {
            ToEmail = email,
            Subject = "[PTDN_QUENMATKHAU] Khôi phục mật khẩu",
            Body = "<div style='font-size: large'>" +
               "<p>Xin chào,</p>" +
               "<p>Mật khẩu mới của bạn là: <b>" + password + "</b></p>" +
               "<p>Vui lòng đổi lại sau khi đăng nhập mật khẩu này.</p>" +
               "<p>Xin cảm ơn!</p>" +
           "</div>"
        };

        await _emailService.SendEmailAsync(mailRequest);
    }
}