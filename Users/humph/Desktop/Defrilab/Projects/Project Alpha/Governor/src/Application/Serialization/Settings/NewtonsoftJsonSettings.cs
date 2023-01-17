
using _.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace _.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}