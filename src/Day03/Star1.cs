using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day03;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day03.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var records = FileReadAllLines(dataPath);

        // Process Data
        BigInteger answer = 0;
        foreach (var (row,  line) in records)
        {
            var rowLargestNumber = GetLargestNumber(line);
            answer += rowLargestNumber;
            logger.LogDebug("row: {row}, largestNumber: {rowLargestNumber}",
                row+1, rowLargestNumber);
        }
        
        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    #region 12/3/2025 - Replaced this function with the function created for Star2
    //BigInteger GetLargestNumber(string line)
    //{
    //    BigInteger result = 0; ;
    //    var lineLength = line.Length;

    //    for (var left = 0; left < lineLength - 1; left++)
    //    {
    //        for(var right = left + 1; right < lineLength; right++)
    //        {
    //            var numberToTest = BigInteger.Parse($"{line[left]}{line[right]}");
    //            result = numberToTest > result ? numberToTest: result;
    //        }
    //    }
    //    return result;
    //}
    #endregion

    record Digit(int Index, byte Value);

    BigInteger GetLargestNumber(string line)
    {
        // we only need to look for the biggest digit in the 
        // set of digits where there can be more digits; ie. we 
        // do not need to look for digits that can not add up to 12 places.
        //
        // in that set, we just find the largest digit and save it.
        //
        // then we move to it's right and continue.

        var lineLength = line.Length;
        var digits = line
            .Select((c, index) => new Digit(index + 1, byte.Parse($"{c}")))
            .ToArray();

        var remainingDigits = 2;

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