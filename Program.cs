using FluentValidation;
using ptdn_net.Configuration;
using ptdn_net.SignalR;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .InitStaticContext()
    ;
builder.Services
    .AddHttpClient()
    .AddValidationService()
    .AddSwaggerConfig()
    .AddDependencyInjectionService()
    .AddAutoMapper(typeof(Program).Assembly)
    .AddRateLimiter()
    .AddCacheConfig()
    .AddValidatorsFromAssembly(typeof(Program).Assembly)
    .AddOptionsConfig(builder.Configuration)
    .AddCorsService(builder.Configuration)
    .AddAuthorizationConfig(builder.Configuration)
    .AddEmailConfig(builder.Configuration)
    .AddDbContext(builder.Configuration)
    .AddControllers();

builder.Services.AddSignalR();

var logPath = builder.Configuration.GetSection("Logging:LogPath").Value ??
              throw new InvalidOperationException("Log path must be specified");
var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logPath)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

    app
        .ConfigCaching()
        .ConfigMiddleware()
        .UseCors("AllowAllCors")
        .UseRateLimiter()
        .UseStaticFiles()
        .ConfigAuthAndAuthor();

    app.MapHub<NotificationHub>("notificationHub");

    app.MapControllers();

    app.Run();