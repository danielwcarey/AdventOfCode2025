using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.DanielCarey.AdventTemplate;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "DanielCarey.AdventTemplate.Star1"; }

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
        WriteLine($"Answer: {answer}");
        return ValueTask.FromResult(answer);
    }
}