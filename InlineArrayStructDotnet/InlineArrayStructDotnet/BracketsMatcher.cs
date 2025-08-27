using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlineArrayStructDotnet;

public static class BracketsMatcher
{
    // Using a generic parameter with a constraint to ICharStack
    // allows us to pass structs that implement the interface
    // without boxing them to the heap.
    public static bool AreBracketsBalanced<TStack>(string input, TStack stack) where TStack : ICharStack
    {
        foreach (char c in input)
        {
            if (c is '(' or '{' or '[')
            {
                stack.Push(c);
            }
            else if (c is ')' or '}' or ']')
            {
                if (stack.Count == 0)
                {
                    // Reached end of input with unmatched brackets
                    return false;
                }

                char open = stack.Pop();
                if (!IsMatchingPair(open, c))
                {
                    return false;
                }
            }
        }

        // Ensure we don't have any remaining unmatched brackets in the stack
        return stack.Count == 0;
    }

    static bool IsMatchingPair(char open, char close) =>
        (open == '(' && close == ')') ||
        (open == '{' && close == '}') ||
        (open == '[' && close == ']');
}
