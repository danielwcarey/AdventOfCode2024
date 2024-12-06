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

    // Modified code based on initial answer to ChatGPT 4o question:
    //   give me a c# expression to generate every permutation of the 
    //   following code snippet. I want to use the Array literals: 
    //   List<int> items = [1, 3, 5, 8, 9, 11];
    public static IEnumerable<IEnumerable<TElement>> Permutations<TElement>(this IEnumerable<TElement> sequence)
    {
        IEnumerable<TElement> enumerable = sequence.ToList();

        if (!enumerable.Any())
        {
            return [[]];
        }

        return enumerable
            .SelectMany((x, i)  // collapse the internal Select to a single list here. also include the index
                => Permutations(
                        enumerable
                            .Take(i)
                            .Concat(enumerable.Skip(i + 1))
                        )
                    .Select(p => p.Prepend(x))
            );
    }
}

