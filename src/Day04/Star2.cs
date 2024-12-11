using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day04;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day04.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var grid = Grid<string>.CreateFromText(File.ReadAllText(dataPath));

        // Process Data

        BigInteger answer = 0;

        for (var row = 0; row < grid.MaxY; row++)
        {
            for (var col = 0; col < grid.MaxX; col++)
            {
                // A is in the center
                if (grid[new(row, col)] != "A") continue;

                // look for MAS, SAM on angle "\"
                bool angle1 =
                    (grid[new(row - 1, col - 1)] == "M" && grid[new(row + 1, col + 1)] == "S")
                    || (grid[new(row - 1, col - 1)] == "S" && grid[new(row + 1, col + 1)] == "M");

                // look for MAS, SAM on angle "/"
                bool angle2 =
                    (grid[new(row - 1, col + 1)] == "M" && grid[new(row + 1, col - 1)] == "S")
                    || (grid[new(row - 1, col + 1)] == "S" && grid[new(row + 1, col - 1)] == "M");

                if (angle1 && angle2)
                {
                    answer += 1;
                }
            }
        }
        // 1960
        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);

    }
}