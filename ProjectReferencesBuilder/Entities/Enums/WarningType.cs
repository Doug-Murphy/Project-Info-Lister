using System.Text.Json.Serialization;

namespace ProjectReferencesBuilder.Entities.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WarningType
    {
        EndOfLife,
        ProjectStyle
    }
}
