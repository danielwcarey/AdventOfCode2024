using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day10;

public record MyUser(string Id, string Name);


public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day10.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        //var grid = Grid<string>.CreateFromText(File.ReadAllText(dataPath));
        var grid = Grid<MyUser>.Create(10, 10, new MyUser("1", "Danie"));

        // Process Data
        //var trailHeads = grid.Where(s => s == "0");

        //foreach (var item in trailHeads)
        //{

        //}

        BigInteger answer = 0;
        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }
}