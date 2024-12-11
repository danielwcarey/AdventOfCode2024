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
            if (map[new(currentLocation.Row, currentLocation.Column)] == "#") return true;

            if (currentLocation == destination) return false;

            currentLocation = new
            (
                Row: currentLocation.Row + movement.Row, Column: currentLocation.Column + movement.Column
            );

        } while (true);

    }
}
