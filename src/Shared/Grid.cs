using System.Numerics;
using System.Text;

namespace DanielCarey.Shared;

/// <summary>
/// Creates a Grid so that we can reference: item[row, col]
/// </summary>
public class Grid<TCell> where TCell : IComparable<TCell>
{
    public required BigInteger MaxX { get; init; }
    public required BigInteger MaxY { get; init; }

    public Dictionary<Point, TCell> GridData { get; } = new();

    public TCell? this[Point point]
    {
        get
        {
            if (point.X < 0 || point.X >= MaxX || point.Y < 0 || point.Y >= MaxY)
            {
                return default;
            }
            return GridData[point];
        }
        set
        {
            if (value is not null)
            {
                GridData[point] = value;
            }
        }
    }

    public bool IsMatch(Point point, TCell value)
    {
        var item = this[point];
        if (item == null) return false;

        return item.CompareTo(value) == 0;
    }

    // search but provide a function that indicates if we can stop searching
    public IEnumerable<(Point point, TCell Value)> Select()
    {
        for (BigInteger y = 0; y < MaxY; y++)
        {
            for (BigInteger x = 0; x < MaxX; x++)
            {
                TCell? value = this[new(x, y)];

                if (value is not null)
                {
                    yield return (new Point(x, y), value);
                }
            }
        }
    }

    public string CreateMap(List<Point> markSpots)
    {
        StringBuilder sb = new();

        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
            {
                var ch = this[new(x, y)] as string;

                var shouldMarkSpot = markSpots.Any(n => n.X == x && n.Y == y);
                if (shouldMarkSpot) ch = "#";

                sb.Append(ch);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static Grid<string> CreateFromText(string text)
    {
        var lines = text.Split(["\r\n"], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var maxY = lines.LongLength;
        var maxX = lines[0].LongCount();

        var result = new Grid<string>
        {
            MaxX = maxX,
            MaxY = maxY
        };

        for (int y = 0; y < maxY; y++)
        {
            for (int x = 0; x < maxX; x++)
            {
                result[new(x, y)] = lines[y][x].ToString();
            }
        }
        return result;
    }
}
