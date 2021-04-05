using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public interface IStringCutter
    {
        string Cut(string str, int maxlength);
    }
}
