using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day08;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day08.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var map = Grid<string>.CreateFromText(File.ReadAllText(dataPath));

        // Process Data

        var items = map.Select().Where(i => i.Value != ".");

        List<Point> antiNodes = new();

        var convertedGroupings = items
            .GroupBy(i => i.Value)
            .Select(g => new { g.Key, Points = g.Select(i => i.point) })
            .ToList();

        // for each unique antennas
        foreach (var itemGroup in convertedGroupings)
        {
            ProcessItemGroup(itemGroup.Points);
        }

        Write(map.CreateMap(antiNodes));

        BigInteger answer = antiNodes.Distinct().Count();
        WriteLine($"Answer {answer}");

        return ValueTask.FromResult(answer);

        void ProcessItemGroup(IEnumerable<Point> itemGroup)
        {
            // each pair (to form a line)
            foreach (var pair in itemGroup.ToList().ToUniquePairs())
            {
                ProcessPair(pair);
            }
        }

        void ProcessPair((Point p1, Point p2) pair )
        {
            var (p1, p2) = pair;

            var dx = BigInteger.Abs(p2.X - p1.X);
            var dy = BigInteger.Abs(p2.Y - p1.Y);

            Point? p3;
            Point? p4;

            if (p1.X < p2.X)
            {
                if (p1.Y < p2.Y)
                {
                    p3 = new(p1.X - dx, p1.Y - dy);
                    p4 = new(p2.X + dx, p2.Y + dy);
                }
                else
                {
                    p3 = new(p1.X - dx, p1.Y + dy);
                    p4 = new(p2.X + dx, p2.Y - dy);
                }
            }
            else // p1.X > p2.X
            {
                if (p1.Y < p2.Y)
                {
                    p3 = new(p1.X + dx, p1.Y - dy);
                    p4 = new(p2.X - dx, p2.Y + dy);
                }
                else
                {
                    p3 = new(p1.X + dx, p1.Y + dy);
                    p4 = new(p2.X - dx, p2.Y - dy);
                }
            }

            if (IsInBounds(p3, map)) antiNodes.Add(p3);
            if (IsInBounds(p4, map)) antiNodes.Add(p4);

        }

        bool IsInBounds(Point point, Grid<string> map)
        {
            return point.X >= 0
                   && point.X < map.MaxX
                   && point.Y >= 0
                   && point.Y < map.MaxY;
        }
    }
   
}