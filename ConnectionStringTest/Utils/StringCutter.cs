using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public class StringCutter : IStringCutter
    {
        public string Cut(string str, int maxlength)
        {
            var nonWordRegex = new Regex("\\W");
            str = str.Substring(0, maxlength);

            int cutoff = -1;

            foreach (Match match in nonWordRegex.Matches(str))
            {
                if(cutoff < match.Index)
                {
                    cutoff = match.Index;
                }
            }

            return str.Substring(0, cutoff) + "...";
        }
    }
}
