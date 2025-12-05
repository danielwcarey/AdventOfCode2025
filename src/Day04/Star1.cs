using System.Numerics;

using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day04;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day04.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data

        using var stream = File.OpenText(dataPath);

        // Process Data
        BigInteger answer = FindRemovableRolls(stream);

        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    record Cell(BigInteger X, BigInteger Y, bool IsRoll, BigInteger NeighborCount);

    BigInteger FindRemovableRolls(StreamReader stream)
    {
        BigInteger answer = 0;
        BigInteger y = 0;
        Cell[] line1 = ToCellArray(stream.ReadLine() ?? "", ++y);
        Cell[] line2 = ToCellArray(stream.ReadLine() ?? "", ++y);

        do
        {
            if (
                line1.Length != line2.Length
                && line1.Length != 0
                && line2.Length != 0
            ) logger.LogError("Different Lengths: line1: {line1}, line2: {line2}",
                line1.Length, line2.Length);

            for (var x = 0; x < line1.Length; x++)
            {
                if(!line1[x].IsRoll) continue;

                var neighborCount = CountNeighbors(x, line1, line2);
                if (neighborCount < 4) answer++;
            }

            line1 = line2;
            line2 = ToCellArray(stream.ReadLine() ?? "", ++y);

        } while (line1.Length > 0);

        logger.LogDebug("Last row: {y}", y);

        return answer;
    }

    // Create Cell without NeighborCount
    Cell[] ToCellArray(string line, BigInteger Y)
    {
        return line.Select((c, index) =>
            new Cell(index, Y, IsRoll: c != '.', 0)
        ).ToArray();
    }

    BigInteger CountNeighbors(int x, Cell[] line1, Cell[] line2)
    {
        var length = line1.Length;
        var cellAdd = 0;

        var canLookEast = x < length - 1;
        var canLookWest = x > 0;
        var canLookSouth = line2.Length > 0;
        var canLookSouthWest = canLookSouth && canLookWest;
        var canLookSouthEast = canLookSouth && canLookEast;

        // look east, update east
        Cell? cellEast = canLookEast && line1[x + 1].IsRoll
            ? line1[x + 1]
            : null;
        if (cellEast != null)
        {
            cellAdd++;
            line1[x + 1] = line1[x + 1] with { NeighborCount = line1[x + 1].NeighborCount + 1 };
        }

        // look southwest, update southwest
        Cell? southwestCell = canLookSouthWest && line2[x - 1].IsRoll
            ? line2[x - 1]
            : null;
        if (southwestCell != null)
        {
            cellAdd++;
            line2[x - 1] = line2[x - 1] with { NeighborCount = line2[x - 1].NeighborCount + 1 };
        }

        // look south, update south
        Cell? southCell = canLookSouth && line2[x].IsRoll
            ? line2[x]
            : null;
        if (southCell != null)
        {
            cellAdd++;
            line2[x] = line2[x] with { NeighborCount = line2[x].NeighborCount + 1 };
        }

        // look southeast, update southeast
        Cell? southeastCell = canLookSouthEast && line2[x + 1].IsRoll
            ? line2[x + 1]
            : null;
        if (southeastCell != null)
        {
            cellAdd++;
            line2[x + 1] = line2[x + 1] with { NeighborCount = line2[x + 1].NeighborCount + 1 };
        }

        line1[x] = line1[x] with { NeighborCount = line1[x].NeighborCount + cellAdd };
        return line1[x].NeighborCount;
    }
}