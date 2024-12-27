using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day08;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day08.Star2"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var map = Grid<string>.CreateFromText(File.ReadAllText(dataPath));

        // Process Data

        var items = map.AsEnumerable().Where(i => i.Value != ".");

        List<BigIntegerPoint> antiNodes = new();

        var antennaGroups =
            items
            .GroupBy(i => i.Value);

        var convertedGroupings = antennaGroups
            .Select(g => new { g.Key, Points = g.Select(i => new BigIntegerPoint(i.X, i.Y)) })
            .ToList();

        //BigInteger antiNodeAntennas = 
        //    new BigInteger(antennaGroups.Count(g => g.Count() > 1));

        // for each unique antennas
        foreach (var itemGroup in convertedGroupings)
        {
            ProcessItemGroup(itemGroup.Points);
        }

        var newMap = map.CreateMap(antiNodes);
        var antiNodeMap = Grid<string>.CreateFromText(newMap);
        Write(newMap);

        var antiNodeAntennas = antiNodeMap
            .AsEnumerable()
            .Where(n => n.Value != "." && n.Value != "#")
            .GroupBy(n => n.Value)
            .SelectMany(n => n)
            .Count();

        BigInteger answer = antiNodes.Distinct().Count() + antiNodeAntennas;
        WriteLine($"Answer {answer}");

        return ValueTask.FromResult(answer);

        void ProcessItemGroup(IEnumerable<BigIntegerPoint> itemGroup)
        {
            var uniquePairs = itemGroup
                .ToList()
                .ToUniquePairs();

            // each pair (to form a line)
            foreach (var pair in uniquePairs)
            {
                ProcessPair(pair);
            }
        }

        void ProcessPair((BigIntegerPoint p1, BigIntegerPoint p2) pair)
        {
            var (p1, p2) = pair;

            var dx = BigInteger.Abs(p2.X - p1.X);
            var dy = BigInteger.Abs(p2.Y - p1.Y);

            BigIntegerPoint? p3;
            BigIntegerPoint? p4;

            Func<BigIntegerPoint, BigIntegerPoint> p3f;
            Func<BigIntegerPoint, BigIntegerPoint> p4f;

            if (p1.X < p2.X)
            {
                if (p1.Y < p2.Y)
                {
                    p3 = new(p1.X - dx, p1.Y - dy);
                    p4 = new(p2.X + dx, p2.Y + dy);

                    p3f = p => new(p.X - dx, p.Y - dy);
                    p4f = p => new(p.X + dx, p.Y + dy);

                }
                else
                {
                    p3 = new(p1.X - dx, p1.Y + dy);
                    p4 = new(p2.X + dx, p2.Y - dy);

                    p3f = p => new(p.X - dx, p.Y + dy);
                    p4f = p => new(p.X + dx, p.Y - dy);
                }
            }
            else // p1.X > p2.X
            {
                if (p1.Y < p2.Y)
                {
                    p3 = new(p1.X + dx, p1.Y - dy);
                    p4 = new(p2.X - dx, p2.Y + dy);

                    p3f = p => new(p.X + dx, p.Y - dy);
                    p4f = p => new(p.X - dx, p.Y + dy);
                }
                else
                {
                    p3 = new(p1.X + dx, p1.Y + dy);
                    p4 = new(p2.X - dx, p2.Y - dy);

                    p3f = p => new(p.X + dx, p.Y + dy);
                    p4f = p => new(p.X - dx, p.Y - dy);
                }
            }

            while (IsInBounds(p3, map))
            {
                antiNodes.Add(p3);
                p3 = p3f(p3);
            }

            while (IsInBounds(p4, map))
            {
                antiNodes.Add(p4);
                p4 = p4f(p4);
            }
        }

        bool IsInBounds(BigIntegerPoint point, Grid<string> map)
        {
            return point.X >= 0
                   && point.X < map.MaxX
                   && point.Y >= 0
                   && point.Y < map.MaxY;
        }
    }
}