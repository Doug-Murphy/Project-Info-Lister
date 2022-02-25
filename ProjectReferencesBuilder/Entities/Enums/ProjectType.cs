using System.Text.Json.Serialization;

namespace ProjectReferencesBuilder.Entities.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProjectType
{
    Pre2017Style,
    SDKStyle,
}