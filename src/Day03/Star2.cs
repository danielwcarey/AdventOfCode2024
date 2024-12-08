using System.Numerics;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day03;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day03.Star2"; }

 
    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var textMemory = File
            .ReadAllText(dataPath);

        // Process Data

        // extract the commands
        var expression =
            @"(?<cmd>mul)\((?<x>\d{1,3})\,(?<y>\d{1,3})\)" +  // mul command
            @"|" +
            @"(?<cmd>do)\(\)" +  // do()
            @"|" +
            @"(?<cmd>don\'t)\(\)";  // don't()

        // normalize the commands
        // (0, _) = don't
        // (1, _) = do
        // (2, x * y) = mul
        var commands = (
                from match in Regex
                    .Matches(textMemory, expression, RegexOptions.None)
                let cmd = match.Groups["cmd"].Value switch
                {
                    "don't" => (Command: 0, Value: 0),
                    "do" => (Command: 1, Value: 0),
                    "mul" => (Command: 2,
                        Value: BigInteger.Multiply(
                            BigInteger.Parse(match.Groups["x"].Value),
                            BigInteger.Parse(match.Groups["y"].Value)
                            )
                        ),
                    _ => throw new Exception("Unknown command")
                }
                select cmd)
            .ToList();

        // process the commands in order. maintaining if we can process
        bool canProcess = true;
        var answer = BigInteger.Zero;

        foreach (var command in commands)
        {
            if (command.Command == 0)
            {
                canProcess = false;
            }
            else if (command.Command == 1)
            {
                canProcess = true;
            }
            else if (canProcess && command.Command == 2)
            {
                answer = BigInteger.Add(answer, command.Value);
            }
        }

        // 100189366
        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }
}