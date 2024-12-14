using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day09;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day09.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var data = File.ReadAllText(dataPath);

        // Process Data
        BigInteger fileId = 0;

        var disk = data.SelectMany(Spread).ToList();
        var maxIndex = disk.Count;

        // find first index of empty space
        var leftIndex = disk.IndexOf(-1); 
        if (leftIndex < 0) throw new Exception("No empty locations");

        // find index of first file from the right
        var rightIndex = disk.FindLastIndex(i => i != -1); 

        do
        {
            disk[leftIndex] = disk[rightIndex];
            disk[rightIndex] = -1;

            leftIndex++;
            rightIndex--;

            while (leftIndex < maxIndex && disk[leftIndex] != -1) leftIndex++;
            while (rightIndex >= 0 && disk[rightIndex] == -1) rightIndex--;

        } while (leftIndex < rightIndex);

        IEnumerable<BigInteger> checksumValues = disk
            .Where(ch => ch != -1)
            .Select((num, index) => BigInteger.Multiply(index, num));

        BigInteger answer = 0;
        foreach (var value in checksumValues) answer = BigInteger.Add(answer, value);

        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);

        List<BigInteger> Spread(char entry, int index)
        {
            bool isFile = index % 2 == 0;
            List<BigInteger> result = [];

            if (isFile)
            {
                result.AddRange(
                    Enumerable
                        .Repeat(
                            BigInteger.Parse($"{fileId}"), // fileId
                            int.Parse($"{entry}")) // count of fileIds
                );
            }
            else
            {
                result.AddRange(
                    Enumerable
                        .Repeat(
                            new BigInteger(-1), // space = -1
                            int.Parse($"{entry}")) // count of fileIds
                );
            }
            if (isFile) fileId++;

            return result;
        }

    }
}