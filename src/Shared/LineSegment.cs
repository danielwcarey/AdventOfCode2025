using System.Numerics;

namespace DanielCarey.Shared;


public class LineSegment(BigIntegerPoint p1, BigIntegerPoint p2)
{
    public BigInteger X1 { get; } = p1.X;
    public BigInteger Y1 { get; } = p1.Y;
    public BigInteger X2 { get; } = p2.X;
    public BigInteger Y2 { get; } = p2.Y;

    public BigInteger Distance()
    {
        // Calculate the direction vector
        BigInteger dx = X2 - X1;
        BigInteger dy = Y2 - Y1;

        // Calculate the length of the segment
        return new BigInteger(Math.Sqrt((double)(dx * dx + dy * dy)));
    }

    public BigIntegerPoint[] GetPointsAtDistance(BigInteger distance)
    {
        // Calculate the direction vector
        BigInteger dx = X2 - X1;
        BigInteger dy = Y2 - Y1;

        // Calculate the length of the segment
        double length = Math.Sqrt((double)(dx * dx + dy * dy));

        // Normalize the direction vector
        double nx = (double)dx / length;
        double ny = (double)dy / length;

        // Scale the normalized vector by the desired distance
        BigInteger scaledX = (BigInteger)(nx * (double)distance);
        BigInteger scaledY = (BigInteger)(ny * (double)distance);

        // Calculate the new points
        BigInteger newX1 = X1 - scaledX;
        BigInteger newY1 = Y1 - scaledY;
        BigInteger newX2 = X2 + scaledX;
        BigInteger newY2 = Y2 + scaledY;

        return [new(newX1, newY1), new(newX2, newY2)];
    }
}
