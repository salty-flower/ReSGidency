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
    QualificationLevel Level, // eg. "大学" with no other information means Bachelor
    string[] InstitutionDescriptors,
    string? Institution,
    string? Major,
    DateOnly? GraduationDate,
    string? Country // eg. "本地大学" would mean Singapore
);
