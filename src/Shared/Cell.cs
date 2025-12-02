using System.Numerics;

namespace DanielCarey.Shared;

public record Cell<TValue>(BigInteger X, BigInteger Y, TValue Value);
