using System.Numerics;
using System.Text.RegularExpressions;

using static DanielCarey.Shared.Extensions;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day13;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day13.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var machines = ParseMachines(File.ReadAllText(dataPath));

        // Process Data
        BigInteger answer = 0;

        foreach (var machine in machines)
        {
            var solutions = Solve(machine).OrderBy(m => m.Cost).ToList();

            if (solutions.Any())
            {
                var solution = solutions.First();
                answer = BigInteger.Add(answer, solution.Cost);
            }
        }

        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }

    public record Solution(BigInteger ButtonACount, BigInteger ButtonBCount)
    {
        public BigInteger Cost { get; private set; } = ButtonACount * 3 + ButtonBCount;
    }

    public class Machine
    {
        public required BigIntegerPoint ButtonA { get; init; }
        public required BigIntegerPoint ButtonB { get; init; }

        public required BigIntegerPoint Prize { get; init; }
    }

    List<Machine> ParseMachines(string text)
    {
        var result = new List<Machine>();

        var machineTextList = text.Split("\r\n\r\n");
        var buttonExpression = @"Button\s*[A|B]:\s*X\+(?<X>\d\d*),\s*Y\+(?<Y>\d\d*)";
        var prizeExpression = @"Prize:\s*X\=(?<X>\d\d*),\s*Y\=(?<Y>\d\d*)";

        foreach (var machineText in machineTextList)
        {
            var lines = machineText.Split("\r\n");
            var buttonAMatch = Regex.Match(lines[0], buttonExpression);
            var buttonBMatch = Regex.Match(lines[1], buttonExpression);
            var prizeMatch = Regex.Match(lines[2], prizeExpression);
            if (!buttonAMatch.Success || !buttonBMatch.Success || !prizeMatch.Success) throw new Exception("Unable to parse");

            result.Add(new Machine()
            {
                ButtonA = new BigIntegerPoint(BigInteger.Parse(buttonAMatch.Groups["X"].Value), BigInteger.Parse(buttonAMatch.Groups["Y"].Value)),
                ButtonB = new BigIntegerPoint(BigInteger.Parse(buttonBMatch.Groups["X"].Value), BigInteger.Parse(buttonBMatch.Groups["Y"].Value)),
                Prize = new BigIntegerPoint(BigInteger.Parse(prizeMatch.Groups["X"].Value), BigInteger.Parse(prizeMatch.Groups["Y"].Value))
            });
        }

        return result;
    }

    // brute force solve
    public List<Solution> Solve(Machine machine)
    {
        List<Solution> solutions = [];

        var maxAX = BigInteger.Add(machine.Prize.X / machine.ButtonA.X, 1);
        var maxAY = BigInteger.Add(machine.Prize.Y / machine.ButtonA.Y, 1);
        var maxA = BigInteger.Max(maxAX, maxAY);

        var maxBX = BigInteger.Add(machine.Prize.X / machine.ButtonB.X, 1);
        var maxBY = BigInteger.Add(machine.Prize.Y / machine.ButtonB.Y, 1);
        var maxB = BigInteger.Max(maxBX, maxBY);

        var result =
            from BigInteger a in range(maxA)
            from BigInteger b in range(maxB)
            let buttonA = machine.ButtonA * a
            let buttonB = machine.ButtonB * b
            where buttonA + buttonB == machine.Prize
            select new Solution
            (
                ButtonACount: a,
                ButtonBCount: b
            );
        return result.ToList();
    }
}