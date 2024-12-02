using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day02;

public class Star1(ILogger<Star1> logger) : IStar
{
    public string Name { get => "Day02.Star1"; }

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        var reports =
            FileReadAllLines("Data1.txt")
            .LoadRecords(ToBigIntegerList);

        // Process Data
        var safeReportCount = reports.Where(IsSafe).Count();

        WriteLine($"Safe Report Count: {safeReportCount}");
        return ValueTask.CompletedTask;
    }

    internal bool IsSafe(List<BigInteger> levels)
    {
        var pairDifferences = levels
            .Zip(levels[1..]) // (i1, i2) (i2, i3) .. (ix-1, ix)
            .Select(pair => pair.First - pair.Second) // difference between pairs: slope
            .ToList();

        var slope = pairDifferences[0] >= 0 ? 1 : -1;

        foreach (var item in pairDifferences)
        {
            var absItem = BigInteger.Abs(item);
            if (absItem < 1 || absItem > 3) return false;

            var itemSlope = item >= 0 ? 1 : -1;
            if (slope != itemSlope) return false;

        }
        return true;
    }
}