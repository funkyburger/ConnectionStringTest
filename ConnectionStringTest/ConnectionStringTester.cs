using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest
{
    public static class ConnectionStringTester
    {
        public static TestResponse Test(string connectionString)
        {
            //return new TestResponse(true, "asdasdasd");
            //return new TestResponse(false, "asdasdasd");

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

            //connetionString = @"Data Source=WIN-50GP30FGO75;Initial Catalog=Demodb;User ID=sa;Password=demol23";
            //cnn = new SqlConnection(connetionString);
            //cnn.Open();
            //MessageBox.Show("Connection Open  !");
            //cnn.Close();
        }
    }
}
