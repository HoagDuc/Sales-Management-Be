using ptdn_net.Common.Exceptions;
using ptdn_net.Utils;

namespace ptdn_net.Configuration;

public static class AppConfig
{
    public static WebApplication ConfigCaching(this WebApplication app)
    {
        var accessor = app.Services.GetService<IHttpContextAccessor>()!;
        SessionUtil.Init(accessor);

        return app;
    }

    public static IApplicationBuilder ConfigMiddleware(this IApplicationBuilder app)
    {
        return app
                .UseMiddleware(typeof(ErrorHandlingMiddleware))
            ;
    }

    public static void ConfigAuthAndAuthor(this IApplicationBuilder app)
    {
        app
            .UseAuthentication()
            .UseAuthorization();
    }
}