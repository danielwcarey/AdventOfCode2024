using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day06;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day06.Star2"; }

    record Location(BigInteger Row, BigInteger Column);

    record Guard(string Symbol, Location Location);

    record Corner(string Name, Location Location); // tl:=top lef, tr:=top right, bl:=bottom left, br:=bootm right

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // ## Setup

        throw new Exception("HAVE NOT SOLVED");

        // Extract Data
        var map = Grid<string>.CreateFromText(File.ReadAllText(dataPath));

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

        // ## Create data for analysis

        // every cell the guard walks in
        List<Location> guardHistory = new();

        // all the corners we find
        List<Corner> corners = new();

        // we loop while the guard is in the grid
        bool guardInArea = true;
        int lastMoveTurnCount = 0;
        guardHistory.Add(new(guard.Location.Row, guard.Location.Column));

        do
        {
            // find guard action from symbol
            var guardAction = guardActions
                .First(action => action.Symbol == guard.Symbol);

            // what is the next location based on the action
            var nextLocation = (
                Row: guard.Location.Row + guardAction.Location.Row,
                Column: guard.Location.Column + guardAction.Location.Column);

            // if the next location is blocked, turn.
            // note: we could turn twice in a row
            if (map[nextLocation.Row, nextLocation.Column] == "#")
            {
                lastMoveTurnCount++;
                guard = guard with { Symbol = TurnSymbol(guard.Symbol) };
                continue; // 
            }

            if (lastMoveTurnCount == 1) // we have found a corner
            {
                Corner newCorner = guard switch
                {
                    { Symbol: "^" } => new(Name: "bl", guard.Location),
                    { Symbol: ">" } => new(Name: "tl", guard.Location),
                    { Symbol: "v" } => new(Name: "tr", guard.Location),
                    { Symbol: "<" } => new(Name: "br", guard.Location),
                    _ => throw new Exception("Guard direction is incorrect")
                };
                corners.Add(newCorner);
            }
            lastMoveTurnCount = 0;

            // move the guard to the next location
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


        // ## Analyze Data
        

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

    bool HasBlockage(Location source, Location destination, Grid<string> map)
    {
        Location movement = new(0, 0);

        if (source.Row == destination.Row)
        {
            if (source.Column < destination.Column) movement = new(0, 1);
            else movement = new(0, -1);
        }
        else if (source.Column == destination.Column)
        {
            if (source.Row < destination.Row) movement = new(-1, 0);
            else movement = new(1, 0);
        }

        if (movement == new Location(0, 0)) return true;

        Location currentLocation = source;
        do
        {
            if (map[currentLocation.Row, currentLocation.Column] == "#") return true;

            if (currentLocation == destination) return false;

            currentLocation = new
            (
                Row: currentLocation.Row + movement.Row, Column: currentLocation.Column + movement.Column
            );

        } while (true);

    }
}
