using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using ptdn_net.Data.Dto.Auth;
using ptdn_net.Services.interfaces;
using ptdn_net.Services.System.Interfaces;
using ptdn_net.SignalR;
using ptdn_net.Utils;

namespace ptdn_net.Controllers.System;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IMemoryCache _memoryCache;

    public AuthController(IAuthService authService, IHubContext<NotificationHub> hubContext,IMemoryCache memoryCache)
    {
        _authService = authService;
        _hubContext = hubContext;
        _memoryCache = memoryCache;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginReq req)
    {
        var user = await _authService.Login(req);
        var token = await _authService.CreateToken(user.Username);
        
        var result = new
        {
            user, token
        };
        await _hubContext.Clients.All.SendAsync("UserLoggedIn", user.Username);
        return Ok(result);
    }
    
    [HttpGet("Logout")]
    public Task<IActionResult> LogOut()
    {
        var username = SessionUtil.GetCurrUsername();
        if (string.IsNullOrEmpty(username))
        {
            throw new InvalidOperationException("User is not logged in");
        }

        _memoryCache.Remove(username);
        return Task.FromResult<IActionResult>(Ok());
    }
    
    [HttpGet("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromQuery] string email)
    {
        await _authService.ForgotPasswordAsync(email);
        return Ok();
    }
    
    // [HttpPost("refresh-token")]
    // public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
    // {
    //     // Kiểm tra tính hợp lệ của refresh token
    //     var principal = _tokenService.GetPrincipalFromExpiredToken(refreshTokenRequest.RefreshToken);
    //     var username = principal.Identity.Name; // Lấy tên người dùng từ refresh token
    //
    //     // Lấy thông tin người dùng từ cơ sở dữ liệu hoặc bộ nhớ cache
    //     var user = await _userService.GetByUsername(username);
    //
    //     // Tạo một JWT mới
    //     var newJwtToken = _tokenService.GenerateToken(user);
    //
    //     return Ok(new { token = newJwtToken });
    // }
}