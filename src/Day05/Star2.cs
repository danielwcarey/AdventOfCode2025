using System.Numerics;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day05;

public class Star2(ILogger<Star2> logger, string dataPath = "Data2.txt") : IStar
{
    public string Name { get => "Day05.Star2"; }

    record Data(BigInteger Num1, BigInteger Num2);

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        MyRange[] myRangeList = [];

        using var stream = new StreamReader(dataPath);

        // Build MyRange[]
        do
        {
            var line = stream.ReadLine()?.Trim() ?? "";
            if (line.Length == 0) break;

            var lineArray = line.Split("-");
            var myRange = new MyRange(
                BigInteger.Parse(lineArray[0]),
                BigInteger.Parse(lineArray[1])
            );
            myRangeList = AddRange(myRange, myRangeList);

        } while (true);

        // Process Data

        BigInteger answer = CountAllGoodItemIds(myRangeList);

        logger.LogInformation("Answer: {answer}", answer);
        return ValueTask.FromResult(answer);
    }

    record MyRange(BigInteger Low, BigInteger High);

    MyRange[] AddRange(MyRange range, MyRange[] list)
    {

        // find MyRange[] left
        MyRange? intersectLow = list.Where(i => range.Low >= i.Low && range.Low <= i.High).FirstOrDefault();
        MyRange? intersectHigh = list.Where(i => range.High >= i.Low && range.High <= i.High).FirstOrDefault();
        MyRange[] myRangeLeft = list.Where(i => i.High < range.Low).Select(i => i).ToArray();
        MyRange[] myRangeRight = list.Where(i => i.Low > range.High).Select(i => i).ToArray();

        // find MyRange[] right
        MyRange[] result = (intersectLow, intersectHigh) switch
        {
            (null, null) => [.. myRangeLeft, range, .. myRangeRight],
            (var x, null) => [.. myRangeLeft, new MyRange(x.Low, range.High), .. myRangeRight],
            (null, var y) => [.. myRangeLeft, new MyRange(range.Low, y.High), .. myRangeRight],
            (var x, var y) => [.. myRangeLeft, new MyRange(x.Low, y.High), .. myRangeRight],
        };

        logger.LogDebug("range: {range}", range);
        logger.LogDebug("intersectLow: {intersectLow}", intersectLow);
        logger.LogDebug("intersectHigh: {intersectHigh}", intersectHigh);
        logger.LogDebug("myRangeLeft: {myRangeLeft}", myRangeLeft);
        logger.LogDebug("myRangeRight: {myRangeRight}", myRangeRight);
        logger.LogDebug("result: {result}", result);

        return result;
    }

    BigInteger CountAllGoodItemIds(MyRange[] myRangeList)
    {
        BigInteger result = 0;

        foreach (var myRange in myRangeList)
        {
            result += (myRange.High - myRange.Low + 1);
        }

        return result;
    }
}