using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public interface IPasswordMasker
    {
        string Mask(string connectionString);
        string ExtractPassword(string connectionString);
    }
}
