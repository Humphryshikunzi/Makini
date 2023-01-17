using System.Text.Json;
using _.Application.Interfaces.Serialization.Options;

namespace _.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}