using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day02;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day02.Star2"; }

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
        foreach (var pair in records)
        {
            foreach (BigInteger invalidId in GetInvalidIds(pair))
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
        var idStringLength = idString.Length;

        var mid = idStringLength / 2; // only need to check up to half characters of length

        var sizesToCheck = Enumerable.Range(1, mid)
            .Where(n => idStringLength % n == 0) // only inspect chunks that will fit length
            .ToArray();

        logger.LogDebug("Sizes to check for {idString}: {sizesToCheck}",
            idString, string.Join(" ", sizesToCheck));

        foreach (var size in sizesToCheck)
        {
            // break into chunks
            // combine the characters into a single string
            // count distinct strings
            var distinctChunkCount = idString
                .Chunk(size)
                .Select(c => 
                    string.Join("", c))
                .Distinct()
                .Count();

            if (distinctChunkCount == 1) // return once we prove id is invalid
            {
                logger.LogDebug("IsInvalid ({size}) for {idString}",
                    size, idString);

                return true; // is invalid
            }
        }
        logger.LogDebug("Is Valid for {idString}", idString);
        return false;
    }
}