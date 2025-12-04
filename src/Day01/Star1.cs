using System.Numerics;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day01;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day01.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        BigInteger position = 50;
        BigInteger totalLandOnZero = 0;
        // Extract Data
        var records = FileReadAllLines(dataPath);

        var regex = new Regex(@"(?<Rotation>\w)(?<Count>\d+)");

        foreach ( var(_, line) in records)
        {
            var match = regex.Match(line);
            var rotation = match.Groups["Rotation"].Value;
            var count = BigInteger.Parse(match.Groups["Count"].Value);

            var startPosition = position;
            position = (rotation) switch
            {
                "L" => (position >= (count % 100) ? position - (count % 100) : 100 + position - (count % 100)),
                "R" => (position + (count % 100)) % 100,
                _ => throw new ArgumentException()
            };
            if (position >= 100) throw new Exception("cannot be > 100");

            logger.LogDebug("Start: {startPosition}, Rotation: {rotation}, Count: {count}, End: {position}",
                startPosition, rotation, count, position);

            if (position == 0) totalLandOnZero++;
        }

        // Process Data

        BigInteger answer = totalLandOnZero;
        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }
}