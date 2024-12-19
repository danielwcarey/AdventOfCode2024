using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day11;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day11.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        List<BigInteger> list = File.ReadAllText(dataPath)
            .Split(' ')
            .Select(n => BigInteger.Parse(n))
            .ToList();

        // Process Data
        var steps = 25;
        while (steps > 0)
        {
            List<BigInteger> nextList = [];

            foreach (var item in list)
            {
                BigInteger[] appendItem = item switch
                {
                    _ when item == 0 => [1],
                    _ when $"{item}".Length % 2 == 0 => SplitNumber(item),
                    _ => [BigInteger.Multiply(item, 2024)],
                };
                nextList.AddRange(appendItem);
            }
            list = nextList;
            steps--;
        }

        BigInteger answer = list.Count;
        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }

    BigInteger[] SplitNumber(BigInteger number)
    {
        var numberText = $"{number}";
        var mid = numberText.Length / 2;
        var num1 = BigInteger.Parse(numberText[..mid]);
        var num2 = BigInteger.Parse(numberText[mid..]);
        return [num1, num2];
    }
}