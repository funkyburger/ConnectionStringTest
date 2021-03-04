using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public static class ConnectionStringTester
    {
        public static TestResponse Test(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return new TestResponse(true, "Connected successfully.");
                }
                catch(Exception e)
                {
                    return new TestResponse(false, e.Message);
                }
            }
        }
    }
}
