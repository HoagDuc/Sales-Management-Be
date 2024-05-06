using System.Text;
using System.Threading.RateLimiting;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ptdn_net.Configuration.Models;
using ptdn_net.Data;
using ptdn_net.Repository;
using ptdn_net.Repository.interfaces;
using ptdn_net.Services;
using ptdn_net.Services.interfaces;
using ptdn_net.Utils;
using ptdn_net.Utils.Email;

namespace ptdn_net.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {
        return services
                .AddEndpointsApiExplorer()
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "PTDN API", Version = "v1" });
                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });

                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    });
                })
                .AddApiVersioning(opt =>
                {
                    opt.ReportApiVersions = true;
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                    opt.DefaultApiVersion = ApiVersion.Default;
                })
            ;
    }

    public static IServiceCollection AddCorsService(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddCors(p =>
            p.AddPolicy("AllowAllCors", build =>
            {
                build
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials();
            }));
    }

    public static IServiceCollection AddValidationService(this IServiceCollection services)
    {
        return services
                .AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters()
            ;
    }

    public static IServiceCollection AddRateLimiter(this IServiceCollection services)
    {
        return services.AddRateLimiter(x => x.AddFixedWindowLimiter("fixedwindow", options =>
        {
            options.Window = TimeSpan.FromSeconds(10);
            options.PermitLimit = 1;
            options.QueueLimit = 0;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        }).RejectionStatusCode = 401);
    }

    public static IConfiguration InitStaticContext(this IConfiguration configuration)
    {
        ConfigUtil.Init(configuration);
        return configuration;
    }

    public static IServiceCollection AddOptionsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        return services
                .Configure<JwtConfig>(configuration.GetSection("JwtConfig"))
            ;
    }

    public static IServiceCollection AddEmailConfig(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"))
            .AddTransient<IEmailService, EmailService>();
    }

    public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtConfig:Issuer"],
                    ValidAudience = configuration["JwtConfig:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:SecretKey"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        return services
                .AddAuthorization()
                .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()
                .AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>()
            ;
    }

    public static IServiceCollection AddCacheConfig(this IServiceCollection services)
    {
        return services
                .AddHttpContextAccessor()
                .AddMemoryCache()
            ;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStr = configuration.GetConnectionString("PostgresqlConnectionStr");
        return services
                .AddDbContextPool<ApplicationDbContext>(options => options
                    .UseNpgsql(connectionStr)
                    .EnableSensitiveDataLogging()
                )
                .AddScoped<IUnitOfWork, UnitOfWork>()
            ;
    }
}