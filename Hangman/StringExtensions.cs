using System.Collections.Generic;

namespace Hangman
{
    public static class StringExtensions
    {
        public static IEnumerable<int> AllIndicesOf(this string str, char c)
        {
            var minIndex = str.IndexOf(c);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(c, minIndex + 1);
            }
        }
    }
}