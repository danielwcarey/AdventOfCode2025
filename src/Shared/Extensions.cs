using System.Numerics;
using System.Text.RegularExpressions;

// When using range function, use it with
// using static DanielCarey.Shared.Extensions;

namespace DanielCarey.Shared;

// Helper methods and extensions
public static partial class Extensions
{
    private static readonly Regex FixSpaces = new(@"\s\s*");

    // Read all file lines and include the line number as the dictionary key
    public static Dictionary<BigInteger, string> FileReadAllLines(string filename)
    {
        return File
            .ReadAllLines(filename)
            .Select((line, index) => (Index: index, Line: line))
            .ToDictionary(item => new BigInteger(item.Index), item => item.Line);
    }

    // Load records from space delimited text
    public static List<TRecord> LoadRecords<TRecord>(
        this Dictionary<BigInteger, string> data,
        Func<string[], TRecord> fieldsToRecord)
    {
        return data
            .Select(item => item.Value)
            .Select(line => FixSpaces.Replace(line, " "))
            .Select(line => fieldsToRecord(line.Split(" ")))
            .ToList(); // lines
    }

    public static List<BigInteger> ToBigIntegerList(string[] items)
        => items.Select(BigInteger.Parse).ToList();

    // Modified code based on initial answer to ChatGPT 4o question:
    //   give me a c# expression to generate every permutation of the 
    //   following code snippet. I want to use the Array literals: 
    //   List<int> items = [1, 3, 5, 8, 9, 11];
    public static IEnumerable<IEnumerable<TElement>> Permutations<TElement>(this IEnumerable<TElement> sequence)
    {
        IEnumerable<TElement> enumerable = sequence.ToList();

        if (!enumerable.Any())
        {
            return [[]];
        }

        return enumerable
            .SelectMany((x, i)  // collapse the internal Select to a single list here. also include the index
                => Permutations(
                        enumerable
                            .Take(i)
                            .Concat(enumerable.Skip(i + 1))
                        )
                    .Select(p => p.Prepend(x))
            );
    }

    public static Stack<TItem> ToStack<TItem>(this IList<TItem> items)
    {
        Stack<TItem> result = new();

        foreach (var item in items.Reverse())
        {
            result.Push(item);
        }
        return result;
    }

    /*
    ChatGPT Question:
    using c# and LINQ, i have a list on ints [1, 3, 5, 7, 9, 11, 13, 21, 23]. I need
    to create a list of unique paris. i also need to create a list of unique triads.
    give me functions for that.
    */

    // { 1, 3, 5, 7, 9, 11, 13, 21, 23 }
    //
    // numbers.ToUniquePairs() -> { (1, 3), (1, 5), (1, 7), ..., (3, 11), (3, 13), ...(13, 23), (21, 23) }
    //
    public static List<(TItem First, TItem Second)> ToUniquePairs<TItem>(this List<TItem> numbers)
    {
        return numbers
            .SelectMany(
                (_, i) => numbers.Skip(i + 1),
                (x, y) => (x, y)
            )
            .ToList();
    }

    // { 1, 3, 5, 7, 9, 11, 13, 21, 23 }
    //
    // numbers.ToUniqueTriads -> { (1, 3, 5), (1, 3, 7), (1, 3, 9), ..., (3, 11, 13), (3, 11, 21), ...(11, 21, 23), (13, 21, 23) }
    //
    public static List<(TItem First, TItem Second, TItem Third)> ToUniqueTriads<TItem>(this List<TItem> numbers)
    {
        // Similar logic to pairs, but extended to three levels:
        // We take an element x at index i, pair it with y from indices > i,
        // and then a z from indices > j, ensuring uniqueness.
        return numbers
            .SelectMany(
                (x, i) => numbers
                    .Skip(i + 1)
                    .SelectMany(
                        (y, j) => numbers
                            .Skip(i + j + 2)
                            .Select(z => (x, y, z))
                    )
            )
            .ToList();
    }


    #region range - like python

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public static IEnumerable<BigInteger> range(BigInteger? stop = null)
    {
        int i = 0;
        while (stop == null || i < stop) yield return i++;
    }

    // ReSharper disable once UnusedMember.Global
    // ReSharper disable once InconsistentNaming
    public static IEnumerable<BigInteger> range(BigInteger start, BigInteger stop, BigInteger? step = null)
    {
        step ??= new BigInteger(1);
        BigInteger i = start;
        while (i < stop)
        {
            yield return i;
            i = BigInteger.Add(i, step.Value);
        }
    }

    #endregion
}

