using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.DanielCarey.AdventTemplate;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "DanielCarey.AdventTemplate.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var records =
            FileReadAllLines(dataPath)
            .LoadRecords(fields
                => new Data(BigInteger.Parse(fields[0]), BigInteger.Parse(fields[1]))
            );

        // Process Data

        BigInteger answer = 0;
        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }
}