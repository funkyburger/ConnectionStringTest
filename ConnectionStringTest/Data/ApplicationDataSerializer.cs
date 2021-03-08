using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class ApplicationDataSerializer : IApplicationDataSerializer
    {
        //private readonly Regex SectionRegex = new Regex( "^[]$");

        public ApplicationData Deserialize(string dataString)
        {
            var currentSection = ApplicationDataFileSection.None;
            var lines = dataString.Split('\n');
            var history = new List<string>();

            foreach(var line in lines.Where(l => !string.IsNullOrEmpty(l)))
            {
                var section = FromLine(line);
                if(section != ApplicationDataFileSection.None)
                {
                    currentSection = section;
                    continue;
                }

                if(currentSection == ApplicationDataFileSection.History)
                {
                    history.Add(line);
                }
                else
                {
                    throw new Exception("Orphan data found.");
                }
            }

            return new ApplicationData(history);
        }

        public string Serialize(ApplicationData data)
        {
            var builder = new StringBuilder();
            if(data == null)
            {
                return builder.ToString();
            }

            if(data.History != null && data.History.Any())
            {
                builder.Append("[History]\n");

                foreach(var history in data.History)
                {
                    builder.Append($"{ history }\n");
                }
            }

            return builder.ToString();
        }

        private ApplicationDataFileSection FromLine(string line)
        {
            if(line.StartsWith("[") && line.EndsWith("]"))
            {
                ApplicationDataFileSection parsedSection;
                if (Enum.TryParse(line.Substring(1, line.Length-2), true, out parsedSection))
                {
                    return parsedSection;
                }
                else
                {
                    throw new Exception("Unrecognized section.");
                }
            }

            return ApplicationDataFileSection.None;
        }
    }
}
