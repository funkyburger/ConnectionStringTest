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

        public string SetPassword(string connectionString, string password)
        {
            var boundaries = GetPasswordBoundaries(connectionString).FirstOrDefault();
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
            var boundaries = GetPasswordBoundaries(connectionString).FirstOrDefault();

            if (boundaries != null)
            {
                return connectionString.Substring(boundaries.Item1, boundaries.Item2);
            }
            return null;
        }

        public string UpdatePassword(string current, string partiallyMasked)
        {
            if(string.IsNullOrEmpty(partiallyMasked))
            {
                return null;
            }

            var builder = new StringBuilder();

            for(int i = 0; i < partiallyMasked.Length; i++)
            {
                if(partiallyMasked[i] == MaskCharacter)
                {
                    if(string.IsNullOrEmpty(current) || i >= current.Length)
                    {
                        throw new PasswordException("Garble left in password");
                    }

                    builder.Append(current[i]);
                }
                else
                {
                    if (string.IsNullOrEmpty(current) || i >= current.Length)
                    {
                        builder.Append(partiallyMasked[i]);
                    }
                    else if (current[i] != partiallyMasked[i])
                    {
                        throw new PasswordException("Partially masked and current passwords don't match.");
                    }
                    else
                    {
                        builder.Append(partiallyMasked[i]);
                    }
                }
            }

            return builder.ToString();
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
