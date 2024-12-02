using System.Numerics;
using System.Text.RegularExpressions;

namespace DanielCarey.Shared;

// Helper methods and extensions
public static class Extensions
{
    private static readonly Regex FixSpaces = new(@"\s\s*");

    // Read all file lines and include the line number as the dictionary key
    public static Dictionary<BigInteger, string> FileReadAllLines(string filename)
    {
        return File
            .ReadAllLines(filename)
            .Select((line, index) => (Index: index, Line: line))
            .ToDictionary(item => new BigInteger(item.Index), item => item.Line);
    }

    // Load records from space delimited text
    public static List<TRecord> LoadRecords<TRecord>(
        this Dictionary<BigInteger, string> data,
        Func<string[], TRecord> fieldsToRecord)
    {
        return data
            .Select(item => item.Value)
            .Select(line => FixSpaces.Replace(line, " "))
            .Select(line => fieldsToRecord(line.Split(" ")))
            .ToList(); // lines
    }

    public static List<BigInteger> ToBigIntegerList(string[] items) 
        => items.Select(BigInteger.Parse).ToList();
    


}

