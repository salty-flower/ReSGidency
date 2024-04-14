namespace ReSGidency.Models;

public record Record(
    // Metadata provided by website
    string UserName,
    DateOnly ApplicationDate,
    DateOnly? EndDate,
    DateOnly UpdateDate,
    ApplicationStatus Status,

    // Attributes parsed from description
    bool? Family,
    bool? Sponsored,
    bool? Male,

    string? Nationality,
    Qualification[]? Qualifications,
    Permit[]? Permits,
    string? Industry,
    int? BaseMonthSalary,
    TimeSpan? DurationInSG,

    int? ApplicationCount
);
