namespace ReSGidency.Models;

public record Record(
    string UserName,
    bool? Family,
    bool? Male,
    string? Industry,
    Qualification[]? Qualifications,
    Permit[]? Permits,
    string? Nationality,
    int? BaseMonthSalary,
    TimeSpan? StayDuration,
    ApplicationStatus Status,
    int? ApplicationCount
);
