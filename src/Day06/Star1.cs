using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day06;

public class Star1(ILogger<Star1> logger) : IStar
{
    public string Name { get => "Day06.Star1"; }

    record Location(BigInteger Row, BigInteger Column);

    record Guard(string Symbol, Location Location);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var map = Grid<string>.CreateFromText(File.ReadAllText("Data1.txt"));

        // guard direction + actions
        List<Guard> guardActions =
        [
            new("^", new(-1, 0) ),
            new(">", new (0, 1) ),
            new("v", new(1, 0) ),
            new("<", new(0, -1) )
        ];

        // get guard
        Guard? guard = map
            .Select() // iterate the map
            .Where(cell => guardActions.Select(g => g.Symbol).Contains(cell.Value)) // find a cell with a guard symbol
            .Select(cell => new Guard(cell.Value, new(cell.Row, cell.Column))) // create the guard object
            .FirstOrDefault();

        if (guard is null) throw new Exception("Cannot find the guard");

        // Process Data
        List<Location> guardHistory = new();
        bool guardInArea = true;
        guardHistory.Add(new(guard.Location.Row, guard.Location.Column));

        do
        {
            var guardAction = guardActions
                .First(action => action.Symbol == guard.Symbol);

            var nextLocation = (
                Row: guard.Location.Row + guardAction.Location.Row,
                Column: guard.Location.Column + guardAction.Location.Column);

            if (map[nextLocation.Row, nextLocation.Column] == "#")
            {
                guard = guard with { Symbol = TurnSymbol(guard.Symbol) };
                continue;
            }

            guard = guard with { Location = new(nextLocation.Row, nextLocation.Column) };

            // determine if the guard is still in the grid
            guardInArea =
                guard.Location.Row >= 0
                && guard.Location.Row < map.Rows
                && guard.Location.Column >= 0
                && guard.Location.Column < map.Columns;

            if (guardInArea)
            {
                Location location = new(guard.Location.Row, guard.Location.Column);
                if (!guardHistory.Contains(location)) // don't recount 
                {
                    guardHistory.Add(location);
                }
            }

        } while (guardInArea);

        // 4982
        WriteLine($"Answer: {guardHistory.Count}");
        return ValueTask.CompletedTask;
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
