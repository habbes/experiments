using System.Numerics;
using System.Runtime.CompilerServices;

namespace DotNetSIMD;

public static class Compute
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MemberWiseSumScalar(int[] A, int[] B, int[] result)
    {
        int size = A.Length;
        
        for (int i = 0; i < size; i++)
        {
            result[i] = A[i] + B[i];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MemberWiseSumSIMD(int[] A, int[] B, int[] result)
    {
        int size = A.Length;

        for (int i = 0; i < size; i += Vector<int>.Count)
        {
            Vector<int> v = new Vector<int>(A, i) + new Vector<int>(B, i);
            v.CopyTo(result, i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ArraySumScalar(int[] A)
    {
        int sum = 0;
        foreach(int value in A)
        {
            sum += value;
        }

        return sum;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ArraySumSIMD(int[] A)
    {
        Vector<int> sums = Vector<int>.Zero;

        for (int i = 0; i < A.Length; i += Vector<int>.Count)
        {
            sums += new Vector<int>(A, i);
        }

        int finalSum = 0;
        for (int n = 0; n < Vector<int>.Count; n++)
        {
            finalSum += sums[n];
        }

        return finalSum;
    }


}
