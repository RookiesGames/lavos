using System.Text.Json;

namespace Lavos.Utils;

public static class JsonHelper
{
    static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static T Deserialize<T>(string json)
    {
        var data = JsonSerializer.Deserialize<T>(json, _options);
        return data;
    }
}