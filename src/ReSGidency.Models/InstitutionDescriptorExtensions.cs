using static ReSGidency.Models.InstitutionDescriptors;

namespace ReSGidency.Models;

public static class InstitutionDescriptorExtensions
{
    // extension method to return string for above enum
    public static string GetString(this Level level) =>
        level switch
        {
            Level.Autonomous => "Autonomous",
            Level.NTU_NUS => "NTU/NUS",
            Level.QS20 => "QS Top 20",
            Level.QS50 => "QS Top 50",
            Level.QS100 => "QS Top 100",
            Level.Top2 => "Top 2",
            Level.C9 => "C9",
            Level.MOE985 => "MOE 985",
            Level.MOE211 => "MOE 211",
            Level.Tier1 => "Tier 1",
            Level.IvyLeague => "Ivy League",
            Level.RussellGroup => "Russell Group",
            Level.Oxbridge => "Oxbridge",
            Level.GoldenTriangle => "Golden Triangle",
            Level.GroupOfEight => "Group of Eight",
            Level.Sandstone => "Sandstone",
            _ => "Unknown"
        };
}
