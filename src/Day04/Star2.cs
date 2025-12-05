using System.Numerics;
using System.Text;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day04;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day04.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        // Read from file first, then build up subsequent text to parse.

        BigInteger answer = 0;
        BigInteger foundRolls = 0;
        string text = string.Empty;
        Func<string> readline;

        using var stream = File.OpenText(dataPath);
        readline = () => stream.ReadLine();

        // Process Data
        (foundRolls, text) = FindRemovableRolls(readline);
        answer += foundRolls;

        do
        {
            using var stringStream = new StringReader(text);
            readline = () => stringStream.ReadLine();
            (foundRolls, text) = FindRemovableRolls(readline);
            answer += foundRolls;

        } while (foundRolls > 0);


        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    record Cell(BigInteger X, BigInteger Y, bool IsRoll, BigInteger NeighborCount);

    string CellText(Cell c) => c.IsRoll ? "@" : ".";

    (BigInteger, string) FindRemovableRolls(Func<string> readLine)
    {
        BigInteger answer = 0;
        BigInteger y = 0;
        Cell[] line1 = ToCellArray(readLine() ?? "", ++y);
        Cell[] line2 = ToCellArray(readLine() ?? "", ++y);
        StringBuilder text = new();

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
                if (!line1[x].IsRoll)
                {
                    text.Append(".");
                    continue;
                }

                var neighborCount = CountNeighbors(x, line1, line2);
                if (neighborCount < 4)
                {
                    text.Append(".");
                    answer++;
                } else
                {
                    text.Append("@");
                }
            }
            text.AppendLine();

            line1 = line2;
            line2 = ToCellArray(readLine() ?? "", ++y);

        } while (line1.Length > 0);

        logger.LogDebug("Last row: {y}", y);

        return (answer, text.ToString());
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