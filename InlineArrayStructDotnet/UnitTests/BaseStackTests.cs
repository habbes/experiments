using InlineArrayStructDotnet;

namespace UnitTests;

/// <summary>
/// Ensures that different implementations of ICharStack produce the same results.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class BaseStackTests<T> where T : ICharStack, new()
{
    protected abstract T CreateStack();

    [Theory]
    [InlineData("()")]
    [InlineData("([])")]
    [InlineData("({[asdfsf( dafsdfdsf ) sfsfdsf]   sfsfdsf})")]
    public void AreBracketsBalanced_ReturnsTrue_IfBracketsAreBalanced(string input)
    {
        Assert.True(BracketsMatcher.AreBracketsBalanced("()", CreateStack()));
    }

    [Theory]
    [InlineData("(")]
    [InlineData("())")]
    [InlineData("({[asdfsf( dafsdfdsf ) sfsfdsf]   sfsfdsf})]")]
    public void AreBracketsBalanced_ReturnsFalse_IfBracketsAreNotBalanced(string input)
    {
        Assert.False(BracketsMatcher.AreBracketsBalanced(input, CreateStack()));
    }

    [Fact]
    public void AreBracketsBalanced_CanHandleLongInput()
    {
        int numBrackets = 100;
        var input = string.Create(numBrackets * 2, numBrackets, static (span, state) =>
        {
            for (int i = 0; i < state; i++)
            {
                span[i] = '(';
                span[span.Length - 1 - i] = ')';
            }

            for (int i = state; i < span.Length; i++)
            {
                span[i] = ')';
            }
        });


        Assert.True(BracketsMatcher.AreBracketsBalanced(input, CreateStack()));
    }
}
