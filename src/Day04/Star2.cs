using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day04;

public class Star2(ILogger<Star2> logger) : IStar
{
    public string Name { get => "Day04.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var grid = Grid<string>.CreateFromText(File.ReadAllText("Data2.txt"));

        // Process Data

        BigInteger foundWords = 0;

        for (var row = 0; row < grid.Rows; row++)
        {
            for (var col = 0; col < grid.Columns; col++)
            {
                // A is in the center
                if (grid[row, col] != "A") continue;
                
                // look for MAS, SAM on angle "\"
                bool angle1 =
                    (grid[row - 1, col - 1] == "M" && grid[row + 1, col + 1] == "S")
                    || (grid[row - 1, col - 1] == "S" && grid[row + 1, col + 1] == "M");

                // look for MAS, SAM on angle "/"
                bool angle2 =
                    (grid[row - 1, col + 1] == "M" && grid[row + 1, col - 1] == "S")
                    || (grid[row - 1, col + 1] == "S" && grid[row + 1, col - 1] == "M");

                if (angle1 && angle2)
                {
                    foundWords += 1;
                }
            }
        }
        // 1960
        WriteLine($"Answer: {foundWords}");
        return ValueTask.CompletedTask;

    }
}