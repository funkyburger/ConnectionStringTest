using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest
{
    public class TestResponse
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }

        public TestResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
