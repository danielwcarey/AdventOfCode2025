using System.Numerics;
using System.Reflection.Metadata.Ecma335;

// ReSharper disable once CheckNamespace
namespace DanielCarey.Day05;

public class Star1(ILogger<Star1> logger, string dataPath = "Data1.txt") : IStar
{
    public string Name { get => "Day05.Star1"; }

    public ValueTask<BigInteger> RunAsync()
    {
        logger.LogInformation("RunAsync");

        // Extract Data
        MyRange[] myRangeList = [];
        List<BigInteger> items = [];

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

        // Build item list
        do
        {
            var line = stream.ReadLine()?.Trim() ?? "";
            if (line.Length == 0) break;
            var item = BigInteger.Parse(line);

            if (items.Contains(item)) continue;
            items.Add(item);

        } while (true);


        // Process Data

        BigInteger answer = CountGoodItems(myRangeList, items.ToArray());

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

    BigInteger CountGoodItems(MyRange[] myRangeList, BigInteger[] items)
    {
        var goodItems =
            from range in myRangeList
            from item in items
            where range.Low <= item && item <= range.High
            select item;

        return goodItems.Count();
    }
}