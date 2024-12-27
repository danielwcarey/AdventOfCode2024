using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day14;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day14.Star1"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var robots = parseRobots(File.ReadAllText(dataPath));
        BigInteger maxX = 11;
        BigInteger maxY = 7;
        BigInteger elapsedTime = 100;

        // Process Data

        var robotFutures = new List<Robot>();
        foreach (Robot robot in robots)
        {
            var newPosition = new BigIntegerPoint(
                X: robot.Position.X * elapsedTime % elapsedTime,
                Y: robot.Position.Y * elapsedTime % elapsedTime);

            robotFutures.Add(robot with { Position = newPosition });
        }


        BigInteger answer = 0;
        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }

    record Robot(BigIntegerPoint Position, BigIntegerPoint Velocity);

    List<Robot> parseRobots(string text)
    {
        var result = new List<Robot>();

        var lines = text.Split("\r\n");
        var expression = @"p=(?<px>\d\d*),(?<py>\d\d*)\s*v=(?<vx>\-?\d\d*),(?<vy>\-?\d\d*)";

        foreach (var line in lines)
        {
            var match = Regex.Match(line, expression);
            if (!match.Success) throw new Exception("Unable to parse line");

            var position = new BigIntegerPoint(
                X: BigInteger.Parse(match.Groups["px"].Value),
                Y: BigInteger.Parse(match.Groups["py"].Value)
            );

            var velocity = new BigIntegerPoint(
                X: BigInteger.Parse(match.Groups["vx"].Value),
                Y: BigInteger.Parse(match.Groups["vy"].Value)
            );

            result.Add(new Robot(Position: position, Velocity: velocity));
        }

        return result;
    }
}