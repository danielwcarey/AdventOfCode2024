using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day02;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day02.Star2"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        var reports =
            FileReadAllLines(dataPath)
                .LoadRecords(ToBigIntegerList);

        // Process Data
        BigInteger answer = reports.Where(IsSafeWithProblemDampener).Count();

        WriteLine($"Safe Report Count: {answer}");
        return ValueTask.FromResult(answer);
    }

    internal bool IsSafeWithProblemDampener(List<BigInteger> levels)
    {
        var badIndex = GetBadPairIndex(levels);
        if (badIndex == null) return true;

        int badIndexValue = (int)badIndex;

        // Only examine the indexes surrounding bad pair
        List<int> listBadIndexes =
        [
            badIndexValue,
            badIndexValue + 1
        ];
        if(badIndexValue > 0) listBadIndexes.Add(badIndexValue - 1);

        // loop through the 2 or 3 indexes to remove and validate
        foreach (var index in listBadIndexes)
        {
            var newLevels = new List<BigInteger>();
            newLevels.AddRange(levels);
            newLevels.RemoveAt(index);

            if (GetBadPairIndex(newLevels) == null) return true;
        }
        return false;
    }

    // Same as Star1 IsSafe but returns the index to erroring pair index
    internal int? GetBadPairIndex(List<BigInteger> levels)
    {
        var pairDifferences = levels
            .Zip(levels[1..]) // (i1, i2) (i2, i3) .. (ix-1, ix)
            .Select(pair => pair.First - pair.Second) // difference between pairs
            .ToList();

        var slope = pairDifferences[0] >= 0 ? 1 : -1;

        for (var index = 0; index < pairDifferences.Count; index++)
        {
            var item = pairDifferences[index];
            var absItem = BigInteger.Abs(item);
            if (absItem < 1 || absItem > 3) return index;

            var itemSlope = item >= 0 ? 1 : -1;
            if (slope != itemSlope) return index;

        }
        return null;
    }
}