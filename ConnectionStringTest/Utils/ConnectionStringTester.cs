﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public class ConnectionStringTester : IConnectionStringTester
    {
        public async Task<TestResponse> Test(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return new TestResponse(true, "Connected successfully.");
                }
            }
            catch (Exception e)
            {
                return new TestResponse(false, e.Message);
            }
        }
    }
}
