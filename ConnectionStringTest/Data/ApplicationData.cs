using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class ApplicationData
    {
        public IEnumerable<string> History { get; private set; }

        public ApplicationData(IEnumerable<string> history)
        {
            History = history;
        }
    }
}
