using System.Text.RegularExpressions;

namespace DanielCarey.Shared;

// Helper methods and extensions
public static class Extensions
{
    private static readonly Regex FixSpaces = new(@"\s\s*");

    // Load records from space delimited text
    public static List<TRecord> LoadRecords<TRecord>(
        this string text,
        Func<string[], TRecord> fieldsToRecord)
    {
        return text
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(line => FixSpaces.Replace(line, " "))
            .Select((line, index) => fieldsToRecord(line.Split(" ")))
            .ToList(); // lines
    }

    
}

