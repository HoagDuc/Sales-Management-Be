namespace ptdn_net.Utils;

public abstract class ConfigUtil
{
    public static readonly string ConnectionStr = Configuration.GetConnectionString("PostgresqlConnectionStr")!;
    private static IConfiguration Configuration { get; set; } = null!;

    /// <summary>
    ///     Set config -> Only run 1 time in Program.cs
    /// </summary>
    /// <param name="configuration"></param>
    public static void Init(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static string GetRootPath()
    {
        return Configuration.GetValue<string>("AppSettings:SaveFilePath")!;
    }
}