using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day03;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day03.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var records = FileReadAllLines(dataPath);

        // Process Data
        BigInteger answer = 0;
        foreach (var (row, line) in records)
        {
            var rowLargestNumber = GetLargestNumber(line);
            answer += rowLargestNumber;
            logger.LogDebug("row: {row}, largestNumber: {rowLargestNumber}",
                row + 1, rowLargestNumber);
        }

        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    record Digit(int Index, byte Value);

    BigInteger GetLargestNumber(string line)
    {
        var lineLength = line.Length;
        var digits = line
            .Select((c, index) => new Digit(index + 1, byte.Parse($"{c}")))
            .ToArray();

        var remainingDigits = 12;

        List<Digit> largestDigits = new();
        do
        {
            var position = largestDigits.Count > 0 ? largestDigits.Last().Index : 0;
            var positionRange = lineLength - remainingDigits + 1;
            var candidateDigits = digits[position..positionRange];

            var bestDigit =
                candidateDigits
                    .Take(positionRange)
                    .OrderByDescending(d => d.Value)
                    .First();

            largestDigits.Add(bestDigit);
            remainingDigits--;
        } while (remainingDigits > 0);

        var largestDigitsArray = largestDigits.Select(d => d.Value).ToArray() ?? [];

        var result = BigInteger.Parse(string.Join("", largestDigits.Select(d => $"{d.Value}")));
        
        return result;
    }

}