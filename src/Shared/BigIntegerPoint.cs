using System.Numerics;

namespace DanielCarey.Shared;

public record BigIntegerPoint(BigInteger X, BigInteger Y) : IComparable<BigIntegerPoint>
{
    public int CompareTo(BigIntegerPoint? other)
    {
        if (X == other?.X && Y == other?.Y) return 0;
        if (X < other?.X && Y < other?.Y) return -1;
        return 1;
    }

    public static BigIntegerPoint operator +(BigIntegerPoint point1, BigIntegerPoint point2)
    {
        return new BigIntegerPoint(
            BigInteger.Add(point1.X, point2.X),
            BigInteger.Add(point1.Y, point2.Y)
        );
    }

    public static BigIntegerPoint operator -(BigIntegerPoint point1, BigIntegerPoint point2)
    {
        return new BigIntegerPoint(
            BigInteger.Subtract(point1.X, point2.X),
            BigInteger.Subtract(point1.Y, point2.Y)
        );
    }

    public static BigIntegerPoint operator *(BigIntegerPoint point, BigInteger m)
    {
        return new BigIntegerPoint(
            BigInteger.Multiply(point.X, m),
            BigInteger.Multiply(point.Y, m)
        );
    }

    public static BigIntegerPoint operator /(BigIntegerPoint point, BigInteger m)
    {
        return new BigIntegerPoint(
            BigInteger.Divide(point.X, m),
            BigInteger.Divide(point.Y, m)
        );
    }
}
