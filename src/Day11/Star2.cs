using System.Collections.Concurrent;
using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day11;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day11.Star2"; }

    record Stone(BigInteger Step, BigInteger Number);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        throw new NotImplementedException("Coming back to star 2");

        // Extract Data
        var steps = 75;
        var stones = File.ReadAllText(dataPath)
            .Split(' ')
            .Select(n => new Stone(steps, BigInteger.Parse(n)))
            .ToArray();

        // Process Data
        BigInteger answer = stones.Length;
        ConcurrentStack<Stone> stack = new();
        stack.PushRange(stones);

        while (stack.Any())
        {
            if (!stack.TryPop(out Stone stone)) continue;

            var number = stone.Number;
            var nextStep = stone.Step - 1;

            var next = ComputeNext(stone.Number);
            if (next.Length > 1) answer += 1;

            if (nextStep > 0)
            {
                stack.Push(new Stone(nextStep, next[0]));
                if (next.Length > 1) stack.Push(new Stone(nextStep, next[1]));
            }
        }

        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }

    BigInteger[] ComputeNext(BigInteger number)
    {
        return number switch
        {
            _ when number == 0 => [1],
            _ when $"{number}".Length % 2 == 0 => SplitNumber(number),
            _ => [BigInteger.Multiply(number, 2024)],
        };
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