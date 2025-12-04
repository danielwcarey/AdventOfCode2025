using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day02;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day02.Star1"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        var records =
            (from pairs in File.ReadAllText(dataPath).Split(",")
             let pairArray = pairs.Split("-")
             select new Data(BigInteger.Parse(pairArray[0]), BigInteger.Parse(pairArray[1]))
             );

        // Process Data
        BigInteger answer = 0;
        foreach ( var pair in records )
        {
            foreach(BigInteger invalidId in GetInvalidIds(pair))
            {
                logger.LogDebug("answer ({answer}) += invalidId ({invalidId})",
                    answer, invalidId);
                answer = answer + invalidId;
            }
        }

        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    IEnumerable<BigInteger> GetInvalidIds(Data data)
    {
        var current = data.Num1;
        var end = data.Num2;

        while (current <= end)
        {
            if (IsInvalidId(current))
            {
                yield return current;
            }
            current++;
        }
    }

    bool IsInvalidId(BigInteger id)
    {
        var idString = id.ToString();

        // can't have odd number of characters
        if (idString.Length % 2 == 1) return false;
        
        var mid = idString.Length / 2;

        var left = idString[..mid];
        var right = idString[mid..];

        var isInvalid = left == right;

        logger.LogDebug("idString={idString} left={left}, right={right}, isInvalid={isInvalid}",
            idString, left, right, isInvalid);

        return isInvalid;
    }
}