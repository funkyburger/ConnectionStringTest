using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    // For UT
    public interface IAutoCompleteStringWithPasswordsCollection
    {
        string this[int index] { get; }
        string GetUnmasked(string connectionString);
    }
}
