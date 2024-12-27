using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day10;

public record Cell(BigIntegerPoint Point, string Value);


public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day10.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var grid = Grid<string>.CreateFromText(File.ReadAllText(dataPath));

        // Process Data
        var trailHeads = grid
            .Where(s => s == "0")
            .Select(g => new Cell(g.Point, g.Value));

        Stack<List<Cell>> trails = [];

        foreach (var trailHead in trailHeads) trails.Push([trailHead]);

        var completedRoutes = new List<List<Cell>>();

        while (trails.Any())
        {
            var trail = trails.Pop();

            var point = trail[^1].Point;
            var value = int.Parse(trail[^1].Value);

            var nextValue = $"{value + 1}";

            if (trail[^1].Value == "9")
            {
                completedRoutes.Add(trail);
                continue;
            }

            List<BigIntegerPoint> nextPoints = [
                point with { Y = point.Y-1 }, point with { Y = point.Y+1 },
                point with { X = point.X-1 }, point with { X = point.X+1 }
            ];

            foreach (BigIntegerPoint nextPoint in nextPoints)
            {
                if (grid[nextPoint] == nextValue) trails.Push([.. trail, new Cell(nextPoint, nextValue)]);
            }
        }

        BigInteger answer = (
            from route in completedRoutes
            select new
            {
                Begin = route[0].Point,
                End = route[^1].Point
            }
        )
            .Distinct()
            .Count();

        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }
}