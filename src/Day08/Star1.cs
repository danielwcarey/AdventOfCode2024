using System.Numerics;
using System.Runtime.ExceptionServices;

using static System.Net.Mime.MediaTypeNames;

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
        //var items = map.Select().Where(i => i.Value == "A");

        List<Point> antiNodes = new();

        // for each unique antennas
        foreach (var itemGroup in items.GroupBy(i => i.Value))
        {
            ProcessItemGroup(itemGroup);
        }

        map.DrawMap(antiNodes);

        BigInteger answer = antiNodes.Distinct().Count();
        WriteLine($"Answer {antiNodes.Count}:{antiNodes.Distinct().Count()}");
        return ValueTask.FromResult(answer);

        void ProcessItemGroup(IGrouping<string, (Point point, string Value)> itemGroup)
        {
            // each pair (to form a line)
            foreach (var pair in itemGroup.ToList().ToUniquePairs())
            {
                ProcessPair(pair);
            }
        }

        void ProcessPair((
            (Point point, string Value) First,
            (Point point, string Value) Second) pair
            )
        {
            var p1 = new Point(pair.First.point.X, pair.First.point.Y);
            var p2 = new Point(pair.Second.point.X, pair.Second.point.Y);

            var lineSegment = new LineSegment(p1, p2);

            var points = lineSegment.GetPointsAtDistance(lineSegment.Distance());

            var l1 = new Point(points[0].X, points[0].Y);
            var l2 = new Point(points[1].X, points[1].Y);

            if (IsInBounds(l1, map)) antiNodes.Add(l1);
            if (IsInBounds(l2, map)) antiNodes.Add(l2);

            //else // \  
            //{
            //    // p1 is top left
            //    var (p1, p2) = pair switch
            //    {
            //        _ when location1.Column < location2.Column => (location1, location2),
            //        _ => (location2, location1)
            //    };
            //    p1 = new(p1.Row - dy, p1.Column - dx);
            //    p2 = new(p2.Row + dy, p2.Column + dx);

            //    //if (IsInBounds(p1, map)) antiNodes.Add(new(p1.Row, p1.Column));
            //    //if (IsInBounds(p2, map)) antiNodes.Add(new(p2.Row, p2.Column));
            //}
        }
    }
    bool IsInBounds(Point point, Grid<string> map)
    {
        return point.X >= 0
               && point.X < map.MaxX
               && point.Y >= 0
               && point.Y < map.MaxY;
    }
}