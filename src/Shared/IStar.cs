using System.Numerics;

namespace DanielCarey.Shared;

public interface IStar
{
    string Name { get; }
    ValueTask<BigInteger> RunAsync();
}
