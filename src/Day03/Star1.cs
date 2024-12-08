using System.Numerics;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day03;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day03.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var textMemory = File
            .ReadAllText(dataPath);

        // Process Data
        var expression = @"mul\((?<x>\d{1,3})\,(?<y>\d{1,3})\)";

        // 155955228
        var answer = (
                from match in Regex
                    .Matches(textMemory, expression, RegexOptions.None)
                let x = BigInteger.Parse(match.Groups["x"].Value)
                let y = BigInteger.Parse(match.Groups["y"].Value)
                select x * y
                )
            .Aggregate(BigInteger.Zero, BigInteger.Add);

        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }
}