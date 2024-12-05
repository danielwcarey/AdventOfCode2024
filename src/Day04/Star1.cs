using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day04;

public class Star1(ILogger<Star1> logger) : IStar
{
    public string Name { get => "Day04.Star1"; }

    public ValueTask RunAsync()
    {
        logger.LogInformation($"RunAsync");

        // Extract Data
        var grid = Grid<string>.CreateFromText(File.ReadAllText("Data1.txt"));

        // Process Data
        List<string> searchText = ["X", "M", "A", "S"];

        List<(int X, int Y)> directionsToSearch =
        [
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 1),
            (1, -1), (1, 0), (1, 1)
        ];

        BigInteger foundWords = 0;

        for (var row = 0; row < grid.Rows; row++)
        {
            for (var col = 0; col < grid.Columns; col++)
            {
                if (grid[row, col] != searchText[0]) continue;

                var wordCount = SearchDirections(searchText[1..], directionsToSearch, grid, row, col);
                foundWords = BigInteger.Add(foundWords, wordCount);
            }
        }
        // 2517
        WriteLine($"Answer: {foundWords}");
        return ValueTask.CompletedTask;
    }

    public BigInteger SearchDirections(
        List<string> searchText,
        List<(int X, int Y)> directions,
        Grid<string> grid, int row, int col)
    {
        BigInteger counter = 0;
        foreach (var direction in directions)
        {
            var count = Search(
                searchText, (direction.X, direction.Y),
                grid, row, col);
            counter = BigInteger.Add(count, counter);
        }
        return counter;
    }

    private BigInteger Search(
        List<string> searchText,
        (int X, int Y) direction,
        Grid<string> grid, int row, int col)
    {
        if (searchText.Count == 0) return 1;

        row += direction.X;
        col += direction.Y;

        var nextGridItem = grid[row, col];

        if (nextGridItem == null) return 0;

        if (searchText[0] != nextGridItem) return 0;

        return Search(searchText[1..], direction, grid, row, col);
    }

}