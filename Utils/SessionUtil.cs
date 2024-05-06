using System.Security.Claims;

namespace ptdn_net.Utils;

public static class SessionUtil
{
    private const string HeaderUserAction = "User-Action";
    private static IHttpContextAccessor _accessor = null!;

    /// <summary>
    /// Set config -> Only run 1 time in Program.cs
    /// </summary>
    /// <param name="accessor"></param>
    public static void Init(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public static string GetCurrUsername()
    {
        return _accessor.HttpContext
            ?.User
            .FindFirst(ClaimTypes.NameIdentifier)
            ?.Value!;
    }

    public static string? GetCurrRequestId()
    {
        return _accessor.HttpContext?.TraceIdentifier;
    }

    public static string? GetCurrFeAction()
    {
        return _accessor.HttpContext?.Request.Headers[HeaderUserAction];
    }

    public static string? GetCurrHttpMethod()
    {
        return _accessor.HttpContext?.Request.Method;
    }

    public static string? GetCurrPath()
    {
        return _accessor.HttpContext?.Request.Path;
    }
}