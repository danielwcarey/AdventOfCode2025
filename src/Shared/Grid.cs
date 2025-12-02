using System.Numerics;
using System.Text;

namespace DanielCarey.Shared;

/// <summary>
/// Creates a Grid so that we can reference: item[x, y]
/// If using a record, make sure it implements IComparable
/// </summary>
/// <example>
/// public record MyRecord(string Name, int Value) : IComparable<MyRecord>
/// {
/// 	public int CompareTo(MyRecord? other)
/// 	{
/// 		if (other is null) return 1;
/// 		return Value.CompareTo(other.Value);
/// 	}
/// }
/// </example>
public class Grid<TValue> where TValue : IComparable<TValue>
{
    public required BigInteger MaxX { get; init; }
    public required BigInteger MaxY { get; init; }

    public Dictionary<BigIntegerPoint, TValue> GridData { get; } = new();

    public TValue? this[BigInteger x, BigInteger y]
    {
        get => this[new(x, y)];
        set => this[new(x, y)] = value;
    }

    public TValue? this[BigIntegerPoint point]
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

    public bool IsMatch(BigIntegerPoint point, TValue value)
    {
        var item = this[point];
        if (item == null) return false;

        return item.CompareTo(value) == 0;
    }

    public IEnumerator<Cell<TValue>> GetEnumerator()
    {
        for (BigInteger y = 0; y < MaxY; y++)
        {
            for (BigInteger x = 0; x < MaxX; x++)
            {
                TValue? value = this[new(x, y)];

                if (value is not null)
                {
                    yield return new Cell<TValue>(X: x, Y: y, Value: value);
                }
            }
        }
    }

    public IEnumerable<Cell<TValue>> AsEnumerable()
    {
        foreach (var item in this)
        {
            yield return item;
        }
    }

    public string Render()
    {
        var sb = new StringBuilder();

        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
            {
                TValue? value = this[new(x, y)];

                if (value is null)
                {
                    sb.Append(".");
                }
                else
                {
                    sb.Append(value.ToString());
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public IEnumerable<(BigIntegerPoint Point, TValue Value)> Where(Func<TValue, bool> isFound)
    {
        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxY; x++)
            {
                var foundValue = this[new(x, y)];

                if (foundValue is not null && isFound(foundValue))
                {
                    yield return (new(X: x, Y: y), Value: foundValue);
                }
            }
        }
    }

    public string CreateMap(List<BigIntegerPoint> markSpots, char? mark = '#')
    {
        StringBuilder sb = new();

        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
            {
                var ch = this[new(x, y)] as string;

                var shouldMarkSpot = markSpots.Any(n => n.X == x && n.Y == y);
                if (shouldMarkSpot) ch = $"{mark}";

                sb.Append(ch);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static Grid<TValue> Create(BigInteger maxX, BigInteger maxY, TValue defaultValue)
    {
        var grid = new Grid<TValue>() { MaxX = maxX, MaxY = maxY };
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                grid[x, y] = defaultValue;
            }
        }
        return grid;
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
