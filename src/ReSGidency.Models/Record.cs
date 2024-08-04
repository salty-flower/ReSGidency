using System.Text.Json.Serialization;

namespace ReSGidency.Models;

[Flags]
[JsonConverter(typeof(JsonNumberEnumConverter<ApplicationModifiers>))]
public enum ApplicationModifiers
{
    None = 0,
    Sponsored = 1,
    WithSon = 2,
    WithDaughter = 4,
    Family = Sponsored | WithSon | WithDaughter,
}

public record struct ApplicationRecord(
    string Username,
    string Description,
    ApplicationStatus Status,
    DateOnly? ApplicationDate,
    DateOnly? DecisionDate,
    DateTime UpdateTime
);

public record struct ApplicationDetail(
    InstitutionDescriptors.Level? Level,
    Permit[] PermitHistory, // Order: new -> old
    Qualification[] Qualifications,
    ApplicationModifiers ModifierFlag,
    // Infer from context. Chinese text might be from China/Malaysia.
    // "国内" in most case is China.
    // Give ISO 3166-1 alpha-3 code.
    string? Nationality,
    string? Industry,
    int? BaseMonthSalary,
    TimeSpan? DurationInSG, // eg. "{来新加坡,来新,在新}\d年{\d个月}", JSON eg. "P1Y2M8D"
    int? FormerFailedApplicationCount
);
