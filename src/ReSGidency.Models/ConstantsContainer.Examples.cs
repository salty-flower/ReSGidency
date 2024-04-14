namespace ReSGidency.Models;

public static partial class ConstantsContainer
{
    public static readonly Record Example1 =
        new(
            "无名氏",
            null,
            null,
            "health services",
            [
                new(
                    QualificationLevel.Bachelor,
                    [InstitutionDescriptors.Level.NTU_NUS.GetString()],
                    null,
                    "Nursery",
                    new(2021, 05, 30),
                    "Singapore"
                )
            ],
            [Permit.EP, Permit.STP],
            "China",
            3000,
            new(365),
            ApplicationStatus.Pending,
            1
        );

    public static readonly Record Example2 =
        new(
            "John Doe",
            true,
            true,
            "education",
            [
                new(
                    QualificationLevel.Master,
                    [InstitutionDescriptors.Level.IvyLeague.GetString()],
                    "Harvard University",
                    "Mathematics",
                    null,
                    "United States"
                ),
                new(
                    QualificationLevel.Bachelor,
                    [InstitutionDescriptors.Level.NTU_NUS.GetString()],
                    null,
                    "Physics",
                    new(2016, 06, 30),
                    "Singapore"
                )
            ],
            [Permit.PEP, Permit.DP],
            "United States",
            5000,
            new(730),
            ApplicationStatus.Successful,
            2
        );

    public static readonly Record Example3 =
        new(
            "Jane Doe",
            null,
            null,
            "finance",
            [
                new(
                    QualificationLevel.Master,
                    [InstitutionDescriptors.Level.RussellGroup.GetString()],
                    "University of Manchester",
                    "Finance",
                    new(2018, 06, 30),
                    "United Kingdom"
                ),
                new(
                    QualificationLevel.Bachelor,
                    [InstitutionDescriptors.Level.QS20.GetString()],
                    "University of Melbourne",
                    "Economics",
                    new(2015, 06, 30),
                    "Australia"
                )
            ],
            [Permit.LTVP, Permit.LTVP_P],
            "Australia",
            null,
            new(365),
            ApplicationStatus.Pending,
            null
        );
}
