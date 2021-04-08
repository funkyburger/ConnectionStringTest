using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Exceptions
{
    public class TestAlreadyRunningException : Exception
    {
        public TestAlreadyRunningException()
            : base("A test is already running.")
        {

        }
    }
}
