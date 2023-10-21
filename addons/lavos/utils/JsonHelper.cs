using System.Text.Json;

namespace Lavos.Utils;

public static class JsonHelper
{
    public static T Deserialize<T>(string json)
    {
        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        var data = JsonSerializer.Deserialize<T>(json, options);
        return data;
    }
}