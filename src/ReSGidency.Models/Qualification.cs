namespace ReSGidency.Models;

public enum QualificationLevel
{
    PhD,
    Master,
    Bachelor,
    Diploma,
    Others
}

public record Qualification(
    QualificationLevel Level,
    string[] InstitutionDescriptors,
    string? Institution,
    string? Major,
    DateTime? GraduationDate,
    string? Country
);
