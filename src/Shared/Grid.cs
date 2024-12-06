using System.Numerics;

namespace DanielCarey.Shared;

/// <summary>
/// Creates a Grid so that we can reference: item[row, col]
/// </summary>
public class Grid<TCell> where TCell : IComparable<TCell>
{
    public required BigInteger Rows { get; init; }
    public required BigInteger Columns { get; init; }

    private Dictionary<(BigInteger Row, BigInteger Column), TCell> GridData { get; } = new();

    public TCell? this[BigInteger row, BigInteger col]
    {
        get
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Columns)
            {
                return default;
            }
            return GridData[(row, col)];
        }
        set
        {
            if (value is not null)
            {
                GridData[(row, col)] = value;
            }
        }
    }

    public bool IsMatch(BigInteger row, BigInteger column, TCell value)
    {
        var item = this[row, column];
        if (item == null) return false;

        return item.CompareTo(value) == 0;
    }


    // search but provide a function that indicates if we can stop searching
    public IEnumerable<(BigInteger Row, BigInteger Column, TCell Value)> Select()
    {
        for (BigInteger row = 0; row < Rows; row++)
        {
            for (BigInteger column = 0; column < Columns; column++)
            {
                TCell? value = this[row, column];

                if(value is not null) yield return (Row: row, Column: column, Value: value);
            }
        }
    }

    public static Grid<string> CreateFromText(string text)
    {
        var lines = text.Split(["\r\n"], StringSplitOptions.None);
        var rows = lines.LongLength;
        var cols = lines[0].LongCount();

        var result = new Grid<string>()
        {
            Rows = rows,
            Columns = cols
        };

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                result[i, j] = lines[i][j].ToString();
            }
        }

        return result;
    }
}
