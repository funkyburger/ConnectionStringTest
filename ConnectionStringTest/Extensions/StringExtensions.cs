using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Extensions
{
    public static class StringExtensions
    {
        public static string OverwriteSegment(this string str, string replacement, int start, int length)
        {
            var stringBuilder = new StringBuilder(str.Substring(0, start));
            stringBuilder.Append(replacement);
            stringBuilder.Append(str.Substring(start + length));

            return stringBuilder.ToString();
        }
    }
}
