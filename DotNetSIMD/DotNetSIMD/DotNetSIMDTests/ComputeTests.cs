using Xunit;
using DotNetSIMD;
using System;

namespace DotNetSIMDTests;

public class ComputeTests
{
    [Fact]
    public void TestMemberWiseSumScalar()
    {
        int[] A = new int[] { 1, 2, 4, 10, 12, 6, 4, 4, 3, 2, 5, 10, 1, 0, 3, 5 };
        int[] B = new int[] { 2, 5, 3, 7, 10, 2, 1, 11, 9, 5, 6, 4, 2, 1, 8, 1 };
        int[] C = new int[16];

        Compute.MemberWiseSumScalar(A, B, C);

        AssertArrayEquals(
            new int[] { 3, 7, 7, 17, 22, 8, 5, 15, 12, 7, 11, 14, 3, 1, 11, 6 },
            C
        );
    }

    [Fact]
    public void TestMemberWiseSumSIMD()
    {
        int[] A = new int[] { 1, 2, 4, 10, 12, 6, 4, 4, 3, 2, 5, 10, 1, 0, 3, 5 };
        int[] B = new int[] { 2, 5, 3, 7, 10, 2, 1, 11, 9, 5, 6, 4, 2, 1, 8, 1 };
        int[] C = new int[16];

        Compute.MemberWiseSumSIMD(A, B, C);

        AssertArrayEquals(
            new int[] { 3, 7, 7, 17, 22, 8, 5, 15, 12, 7, 11, 14, 3, 1, 11, 6 },
            C
        );
    }

    [Fact]
    public void TestArraySumScalar()
    {
        int[] values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        int result = Compute.ArraySumScalar(values);

        Assert.Equal(136, result);
    }

    [Fact]
    public void TestArraySumSIMD()
    {
        int[] values = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

        int result = Compute.ArraySumSIMD(values);

        Assert.Equal(136, result);
    }

    private static void AssertArrayEquals(int[] expected, int[] actual)
    {
        Assert.Equal(expected.Length, actual.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }
}
