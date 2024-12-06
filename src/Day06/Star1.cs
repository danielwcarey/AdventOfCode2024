using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day06;

public class Star1(ILogger<Star1> logger) : IStar
{
    public string Name { get => "Day06.Star1"; }

    record Guard(string Symbol, BigInteger Row, BigInteger Column);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var map = Grid<string>.CreateFromText(File.ReadAllText("Data1.txt"));

        // guard direction + actions
        List<Guard> guardActions =
        [
            new("^", -1, 0),
            new(">", 0, 1),
            new("v", 1, 0),
            new("<", 0, -1)
        ];

        // get guard
        Guard? guard = map
            .Select() // iterate the map
            .Where(cell => guardActions.Select(g => g.Symbol).Contains(cell.Value)) // find a cell with a guard symbol
            .Select(cell => new Guard(cell.Value, cell.Row, cell.Column)) // create the guard object
            .FirstOrDefault();

        if (guard is null) throw new Exception("Cannot find the guard");

        // Process Data
        List<(BigInteger, BigInteger)> traveled = new();
        bool guardInArea = true;
        traveled.Add((guard.Row, guard.Column));

        do
        {
            var guardAction = guardActions.Where(action => action.Symbol == guard.Symbol).First();
            var nextLocation = (
                Row: guard.Row + guardAction.Row, 
                Column: guard.Column + guardAction.Column);

            if (map[nextLocation.Row, nextLocation.Column] == "#")
            {
                guard = guard with {Symbol = TurnSymbol(guard.Symbol)};
                continue;
            }
            
            guard = guard with {Row = nextLocation.Row, Column = nextLocation.Column };

            guardInArea = (guard.Row >= 0 && guard.Row < map.Rows)
                          && (guard.Column >= 0 && guard.Column < map.Columns);

            // TBD: need to find something other than List<T> that only adds if doesn't exist
            if (guardInArea)
            {
                if (!traveled.Contains((guard.Row, guard.Column))) // don't recount 
                {
                    traveled.Add( (guard.Row, guard.Column) );
                }
            }

        } while (guardInArea);

        // 4982
        WriteLine($"Answer: {traveled.Count}");
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