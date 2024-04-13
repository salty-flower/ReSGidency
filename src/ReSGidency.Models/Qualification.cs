namespace ReSGidency.Models
{
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
        string? Institution,
        string? Major,
        DateTime? GraduationDate
    );
}
