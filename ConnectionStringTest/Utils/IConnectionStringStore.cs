using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public interface IConnectionStringStore
    {
        IEnumerable<string> GetConnectionStrings();
        bool TryGetConnectionStringWithPassword(string connectionString, out string result);
    }
}
