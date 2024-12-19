using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day12;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day12.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var textGrid = Grid<Plot>
            .CreateFromText(File.ReadAllText(dataPath));

        // Process Data
        // Create grid with edges
        var grid = new Grid<Plot>() { MaxX = textGrid.MaxX, MaxY = textGrid.MaxY };
        for (var y = 0; y < textGrid.MaxY; y++)
        {
            for (var x = 0; x < textGrid.MaxX; x++)
            {
                var plant = textGrid[x, y];
                var up = textGrid[x, y - 1] != plant;
                var right = textGrid[x + 1, y] != plant;
                var down = textGrid[x, y + 1] != plant;
                var left = textGrid[x - 1, y] != plant;

                grid[x, y] = new Plot(x, y, textGrid[x, y] ?? "", up, right, down, left, false);
            }
        }

        BigInteger totalCost = 0;
        // Loop through plants, process the regions
        while (true)
        {
            var plot = grid
                .AsEnumerable()
                .FirstOrDefault(g => g.Value.Plant != ".");

            if (plot == null) break; // no more plants to process

            var region = ExtractRegion(plot.X, plot.Y, grid);

            var (area, perimeter) = ComputeAreaAndPerimeter(region);

            BigInteger plotCost = area * perimeter;
            totalCost = BigInteger.Add(totalCost, plotCost);
        }

        WriteLine($"Answer: {totalCost}");
        return ValueTask.FromResult(totalCost);
    }

    public record Plot(BigInteger X, BigInteger Y, string Plant,
        bool UpEdge, bool RightEdge, bool DownEdge, bool LeftEdge,
        bool IsCounted) : IComparable<Plot>
    {
        public int CompareTo(Plot? other)
        {
            if (other is null) return 1;

            if (X == other.X && Y == other.Y) return 0;

            if (X < other.X && Y < other.Y) return -1;

            return 1;
        }
        public override string ToString()
        {
            return Plant;
        }
    }

    List<Plot> ExtractRegion(BigInteger x, BigInteger y, Grid<Plot> grid)
    {
        List<Plot> area = new();

        var startPlot = grid[x, y];
        if (startPlot == null) return area;

        Stack<Plot> search = new();
        search.Push(startPlot);

        while (search.Any())
        {
            var plot = search.Pop();
            if (plot.Plant == ".") continue;

            var sx = plot.X;
            var sy = plot.Y;

            area.Add(plot);
            grid[sx, sy] = plot with { Plant = "." };

            if (plot.UpEdge == false) Add(grid[sx, sy - 1]);
            if (plot.RightEdge == false) Add(grid[sx + 1, sy]);
            if (plot.DownEdge == false) Add(grid[sx, sy + 1]);
            if (plot.LeftEdge == false) Add(grid[sx - 1, sy]);
        }
        return area;

        void Add(Plot? p)
        {
            if (p == null) return;
            if (p.IsCounted) return;
            grid[p.X, p.Y] = grid[p.X, p.Y]! with { IsCounted = true };
            search.Push(p);
        }
    }

    (BigInteger Area, BigInteger Perimeter) ComputeAreaAndPerimeter(List<Plot> region)
    {
        BigInteger area = 0;
        BigInteger perimeter = 0;

        foreach (var plot in region)
        {
            area++;
            if (plot.UpEdge) perimeter++;
            if (plot.RightEdge) perimeter++;
            if (plot.DownEdge) perimeter++;
            if (plot.LeftEdge) perimeter++;
        }
        return (Area: area, Perimeter: perimeter);
    }


}