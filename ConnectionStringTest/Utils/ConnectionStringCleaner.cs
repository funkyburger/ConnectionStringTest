using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public class ConnectionStringCleaner : IConnectionStringCleaner
    {
        public string Clean(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            return connectionString
                .Replace("\\\\", "\\")
                .Replace("\\\"", "\"");
        }
    }
}
