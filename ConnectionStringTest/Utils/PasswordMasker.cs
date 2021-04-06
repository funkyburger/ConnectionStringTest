using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public class PasswordMasker : IPasswordMasker
    {
        public string Mask(string connectionString)
        {
            var boundaries = GetHiddenTextBoudaries(connectionString);

            if(boundaries != null)
            {
                return Mask(connectionString, boundaries);
            }

            return connectionString;
        }

        private Tuple<int, int> GetHiddenTextBoudaries(string connectionString)
        {
            var passwordSegmentRegex = new Regex("(^|;)\\s*Password\\s*=\\s*(?<password>[^\";\\s]+)($|(\\s*($|;)))");

            var matches = passwordSegmentRegex.Matches(connectionString);

            foreach(Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    if(group.Name == "password")
                    {
                        return new Tuple<int, int>(group.Index, group.Length);
                    }
                }
            }

            var passwordWithQuotesSegmentRegex = new Regex("(^|;)\\s*Password\\s*=\\s*\"(?<password>[^\"]+)($|(\"\\s*($|;)))");
            matches = passwordWithQuotesSegmentRegex.Matches(connectionString);

            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    if (group.Name == "password")
                    {
                        return new Tuple<int, int>(group.Index, group.Length);
                    }
                }
            }

            return null;
        }

        private string Mask(string connectionString, Tuple<int, int> boudaries)
        {
            var builder = new StringBuilder(connectionString.Substring(0, boudaries.Item1));
            for(int i = 0; i <= boudaries.Item2; i++)
            {
                builder.Append('\u25CF');
            }

            builder.Append(connectionString.Substring(boudaries.Item1 + boudaries.Item2));
            return builder.ToString();
        }
    }
}
