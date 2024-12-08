using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day02;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day02.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        var reports =
            FileReadAllLines(dataPath)
            .LoadRecords(ToBigIntegerList);

        // Process Data
        BigInteger answer = reports.Where(IsSafe).Count();

        WriteLine($"Safe Report Count: {answer}");
        return ValueTask.FromResult(answer);
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