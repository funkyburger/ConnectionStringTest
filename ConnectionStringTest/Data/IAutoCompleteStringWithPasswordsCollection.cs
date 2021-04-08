using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public interface IAutoCompleteStringWithPasswordsCollection
    {
        string this[int index] { get; }
        [Obsolete]
        bool GetUnmasked(string connectionString, out string unmasked);
        string GetUnmasked(string connectionString);
    }
}
