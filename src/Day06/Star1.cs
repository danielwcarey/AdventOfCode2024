using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day06;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day06.Star1"; }

    record Guard(string Symbol, Point Point);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var map = Grid<string>.CreateFromText(File.ReadAllText(dataPath));

        // guard direction + actions
        List<Guard> guardActions =
        [
            new("^", new(0, -1 ) ),
            new(">", new (1, 0) ),
            new("v", new(0, 1 ) ),
            new("<", new(-1, 0 ) )
        ];

        // get guard
        Guard? guard = map
            .AsEnumerable() // iterate the map
            .Where(cell => guardActions.Select(g => g.Symbol).Contains(cell.Value)) // find a cell with a guard symbol
            .Select(cell => new Guard(cell.Value, new(cell.X, cell.Y))) // create the guard object
            .FirstOrDefault();

        if (guard is null) throw new Exception("Cannot find the guard");

        // Process Data
        List<Point> guardHistory = new();
        bool guardInArea = true;
        guardHistory.Add(new(guard.Point.X, guard.Point.Y));

        do
        {
            var guardAction = guardActions
                .First(action => action.Symbol == guard.Symbol);

            var nextLocation = (
                X: guard.Point.X + guardAction.Point.X,
                Y: guard.Point.Y + guardAction.Point.Y
                );

            if (map[new(nextLocation.X, nextLocation.Y)] == "#")
            {
                guard = guard with { Symbol = TurnSymbol(guard.Symbol) };
                continue;
            }

            guard = guard with { Point = new(nextLocation.X, nextLocation.Y) };

            // determine if the guard is still in the grid
            guardInArea =
                guard.Point.X >= 0
                && guard.Point.X < map.MaxX
                && guard.Point.Y >= 0
                && guard.Point.Y < map.MaxY;

            if (guardInArea)
            {
                Point point = new(guard.Point.X, guard.Point.Y);
                if (!guardHistory.Contains(point)) // don't recount 
                {
                    guardHistory.Add(point);
                }
            }

        } while (guardInArea);

        // 4982
        WriteLine($"Answer: {guardHistory.Count}");
        return ValueTask.FromResult(new BigInteger(guardHistory.Count));
    }

    string TurnSymbol(string currentSymbol)
        => currentSymbol switch
        {
            "^" => ">",
            ">" => "v",
            "v" => "<",
            "<" => "^",
            _ => currentSymbol
        };

}
