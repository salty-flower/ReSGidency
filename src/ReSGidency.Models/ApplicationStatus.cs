using System.Text.Json.Serialization;

namespace ReSGidency.Models;

[JsonConverter(typeof(JsonStringEnumConverter<ApplicationStatus>))]
public enum ApplicationStatus
{
    Unspecified,
    Pending,
    Successful,
    Failed
}
