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
        private const char MaskCharacter = '\u25CF';

        public string Mask(string connectionString)
        {
            var boundaries = GetPasswordBoundaries(connectionString);

            if(boundaries.Any())
            {
                return Mask(connectionString, boundaries);
            }

            return connectionString;
        }

        public string AddGarble(string connectionString)
        {
            var previous = 'a';
            var builder = new StringBuilder();

            foreach(var c in connectionString)
            {
                if(c != MaskCharacter && previous == MaskCharacter)
                {
                    builder.Append(MaskCharacter);
                }

                builder.Append(c);
                previous = c;
            }

            if (previous == MaskCharacter)
            {
                builder.Append(MaskCharacter);
            }

            return builder.ToString();
        }

        private IEnumerable<Tuple<int, int>> GetPasswordBoundaries(string connectionString)
        {
            var passwordSegmentRegex = new Regex("(^|;)\\s*Password\\s*=\\s*(?<password>[^\";\\s]+)($|(\\s*($|;)))");

            var matches = passwordSegmentRegex.Matches(connectionString);

            foreach(Match match in matches)
            {
                foreach (Group group in match.Groups)
                {
                    if(group.Name == "password")
                    {
                        yield return new Tuple<int, int>(group.Index, group.Length);
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
                        yield return new Tuple<int, int>(group.Index, group.Length);
                    }
                }
            }
        }

        private string Mask(string connectionString, Tuple<int, int> boundaries)
        {
            var maskBuilder = new StringBuilder();
            for (int i = 0; i < boundaries.Item2; i++)
            {
                maskBuilder.Append(MaskCharacter);
            }

            return connectionString.OverwriteSegment(maskBuilder.ToString(), boundaries.Item1, boundaries.Item2);
        }

        private string Mask(string connectionString, IEnumerable<Tuple<int, int>> boundaries)
        {
            foreach (var boundary in boundaries)
            {
                connectionString = Mask(connectionString, boundary);
            }

            return connectionString;
        }
    }
}
