namespace ReSGidency.Models;

public abstract class InstitutionDescriptors
{
    public enum Level
    {
        // Singapore

        Autonomous,
        NTU_NUS, // eg. "新二"

        // Malaysia


        // General
        QS20,
        QS50,
        QS100,

        // Chinese
        Top2, // eg. "清北"
        C9,
        MOE985, // eg. "985", "985本", "985硕士", "9本"
        MOE211, // eg. "211本"
        Tier1, // eg. "一本", "双非一本"
        Tier2, // eg. "二本"

        // United States
        IvyLeague,

        // United Kingdom
        RussellGroup,
        Oxbridge, // eg. "牛剑"
        GoldenTriangle,

        // Australia
        GroupOfEight, // eg. "八大", "澳八大"
        Sandstone,

        // Coming soon...
    }
}
