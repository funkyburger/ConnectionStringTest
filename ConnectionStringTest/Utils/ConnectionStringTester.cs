using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public static class ConnectionStringTester
    {
        public static async Task<TestResponse> Test(string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
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
