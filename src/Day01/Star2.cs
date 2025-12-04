using System.Numerics;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day01;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day01.Star2"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        int position = 50;
        int totalLandOnZero = 0;
        // Extract Data
        var records = FileReadAllLines(dataPath);

        var regex = new Regex(@"(?<Rotation>\w)(?<Count>\d+)");

        foreach (var (_, line) in records)
        {
            var match = regex.Match(line);
            var rotation = match.Groups["Rotation"].Value;
            var count = int.Parse(match.Groups["Count"].Value);

            var startPosition = position;
            position = (rotation) switch
            {
                "L" => position >= (count % 100)
                    ? position - (count % 100)
                    : 100 + position - (count % 100),
                "R" => (position + (count % 100)) % 100,
                _ => throw new ArgumentException()
            };
            if (position >= 100) throw new Exception("cannot be > 100");

            logger.LogDebug("Start: {startPosition}, Rotation: {rotation}, Count: {count}, End: {position}",
                    startPosition, rotation, count, position);

            totalLandOnZero += PassThruZero(rotation, startPosition, count);
        }

        // Process Data

        BigInteger answer = totalLandOnZero;
        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    internal int PassThruZero(string rotation, int position, int count)
    {
        return rotation switch
        {
            //"L" => PassThruZeroLeft(position, count),
            //"R" => PassThruZeroRight(position, count),
            "L" => FastPassThruZeroLeft(position, count),
            "R" => FastPassThruZeroRight(position, count),
            _ => throw new Exception("Invalid rotation")
        };
    }

    internal int FastPassThruZeroLeft(int position, int count)
    {
        int result = 0;
        if (position != 0 && position - (count % 100) < 1) result = 1;
        return result += count / 100;
    }

    internal int FastPassThruZeroRight(int position, int count)
    {
        int result = 0;
        if (position + (count % 100) > 99) result = 1;
        return result += count / 100;
    }

    #region Brute Force and Math Comparison
    internal int PassThruZeroLeft(int position, int count)
    {
        // Using Brute Force
        int result = 0;
        int counter = count;
        int computedPosition = position;

        while (counter > 0)
        {
            computedPosition -= 1;
            if (computedPosition == 0) result++;
            if (computedPosition < 0) computedPosition = 99;
            counter--;
        }

        logger.LogInformation("L, Position {position}, Count {count}, result {result}",
            position, count, result);

        // Using Math
        int result2 = 0;
        if (position != 0 && position - (count % 100) < 1) result2 = 1;
        result2 += count / 100;

        // Formula test
        bool isResult2Fail = result2 != result;

        if (isResult2Fail) logger.LogError("result: {result} != result2:{result2}",
            result, result2);

        //if (isResult2Fail) throw new Exception();

        return result2;
    }

    internal int PassThruZeroRight(int position, int count)
    {
        // Using Brute Force
        int result = 0;
        int counter = count;
        int computedPosition = position;

        while (counter > 0)
        {
            computedPosition += 1;
            if (computedPosition == 100) computedPosition = 0;
            if (computedPosition == 0) result++;
            counter--;
        }

        logger.LogInformation("R, Position {position}, Count {count}, result {result}", 
            position, count, result);

        // Using Math
        int result2 = 0;
        if( position + (count % 100) > 99) result2 = 1;
        result2 += count / 100;

        // Formula test
        bool isResult2Fail = result2 != result;

        if (isResult2Fail) logger.LogError("result: {result} != result2:{result2}",
            result, result2);

        if (isResult2Fail) throw new Exception();

        return result;
    }
    #endregion
}