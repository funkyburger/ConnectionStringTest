using System;
using System.Data.SqlClient;

namespace ConnectionStringTest
{
    public static class ConnectionStringTestingService
    {
        public static TestResponse Test(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        return new TestResponse(true, "Connected successfully.");
                    }
                    catch (Exception e)
                    {
                        return new TestResponse(false, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                return new TestResponse(false, e.Message); 
            }
           
        }
    }
}