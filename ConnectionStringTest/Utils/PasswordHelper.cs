using ConnectionStringTest.Exceptions;
using ConnectionStringTest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public class PasswordHelper : IPasswordHelper
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

        public string SetPassword(string connectionString, string password)
        {
            var boundaries = GetHiddenTextBoudaries(connectionString);
            if(boundaries == null)
            {
                if(password == null)
                {
                    return connectionString;
                }
                throw new PasswordException("No slot found for password in connection string.");
            }

            return connectionString.OverwriteSegment(password, boundaries.Item1, boundaries.Item2);
        }

        public string ExtractPassword(string connectionString)
        {
            var boundaries = GetHiddenTextBoudaries(connectionString);

            if (boundaries != null)
            {
                return connectionString.Substring(boundaries.Item1, boundaries.Item2);
            }
            return null;
        }

        public string UpdatePassword(string current, string partiallyMasked)
        {
            throw new NotImplementedException();
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
            var maskBuilder = new StringBuilder();
            for (int i = 0; i <= boudaries.Item2; i++)
            {
                maskBuilder.Append('\u25CF');
            }

            return connectionString.OverwriteSegment(maskBuilder.ToString(), boudaries.Item1, boudaries.Item2);
        }
    }
}
