using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.UI
{
    public interface ILinkedToMainTestControl
    {
        IMainTestControl MainTestControl { get; }
    }
}
