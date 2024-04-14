namespace ReSGidency.Models;

public record Record(
    // Metadata provided by website
    string UserName,
    DateOnly ApplicationDate,
    DateOnly? EndDate,
    DateTime UpdateDate,
    ApplicationStatus Status,
    // Attributes parsed from description
    bool? Family,
    bool? Sponsored,
    bool? Male,
    string? Nationality, // Infer from context. Chinese text might be from China/Malaysia. "国内" in most case is China. Prefer adjective form.
    Qualification[]? Qualifications,
    Permit[]? Permits, // Order: new -> old
    string? Industry,
    int? BaseMonthSalary,
    TimeSpan? DurationInSG, // e.g. "{来新加坡,来新,在新}\d年{\d个月}"
    int? ApplicationCount
);
