using Newtonsoft.Json;

namespace ptdn_net.Utils;

public static class JsonUtil
{
    public static string? ToStr(object? obj)
    {
        return obj == null
            ? null
            : JsonConvert.SerializeObject(obj);
    }

    public static string ToStrIgnoreNull(object obj)
    {
        var options = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.SerializeObject(obj, options);
    }
}