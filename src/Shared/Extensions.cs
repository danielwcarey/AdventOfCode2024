using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace DanielCarey;

public static class Extensions
{
    private static readonly Regex FixSpaces = new(@"\s\s*");

    public static List<TRecord> LoadRecords<TRecord>(
        this string text,
        Func<string[], TRecord> fields)
    {
        return text
            .Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(line => FixSpaces.Replace(line, " "))
            .Select(line => fields(line.Split(" ")))
            .ToList(); // lines
    }
}

