using System.Numerics;

namespace DanielCarey.Shared;

public static partial class Extensions
{

    /// <summary>
    /// Solve linear equation when the coefficent are BigInteger
    /// </summary>
    /// <param name="coefficients"></param>
    /// <param name="constants"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    static BigInteger[] SolveLinearEquations(BigInteger[,] coefficients, BigInteger[] constants)
    {
        int rowCount = coefficients.GetLength(0);
        int colCount = coefficients.GetLength(1);

        var augmentedMatrix = new BigInteger[rowCount, colCount + 1];

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < colCount; j++)
            {
                augmentedMatrix[i, j] = coefficients[i, j];
            }
            augmentedMatrix[i, colCount] = constants[i];
        }

        // Perform Gaussian elimination
        for (int i = 0; i < rowCount; i++)
        {
            for (int j = i + 1; j < rowCount; j++)
            {
                if (augmentedMatrix[i, i] == 0)
                {
                    throw new ArgumentException("Matrix is singular and cannot be solved.");
                }

                BigInteger factor = augmentedMatrix[j, i] / augmentedMatrix[i, i];
                for (int k = i; k <= colCount; k++)
                {
                    augmentedMatrix[j, k] -= factor * augmentedMatrix[i, k];
                }
            }
        }

        // Back substitution
        var solution = new BigInteger[colCount];
        for (int i = rowCount - 1; i >= 0; i--)
        {
            solution[i] = augmentedMatrix[i, colCount];
            for (int j = i + 1; j < colCount; j++)
            {
                solution[i] -= augmentedMatrix[i, j] * solution[j];
            }
            solution[i] /= augmentedMatrix[i, i];
        }

        return solution;
    }
}