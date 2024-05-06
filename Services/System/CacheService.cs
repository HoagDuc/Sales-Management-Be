using Microsoft.Extensions.Caching.Memory;
using ptdn_net.Data.Entity.SystemEntity;
using ptdn_net.Services.System.Interfaces;

namespace ptdn_net.Services.System;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IUserService _userService;

    public CacheService(
        IMemoryCache memoryCache,
        IUserService userService
    )
    {
        _userService = userService;
        _memoryCache = memoryCache;
    }

    public async Task<User> GetUser(string username)
    {
        if (_memoryCache.TryGetValue<User>(username, out var user)) return user!;

        user = await _userService.GetByUsername(username);
        _memoryCache.Set(username, user, DateTimeOffset.Now.AddHours(1));

        return user;
    }
}