using ConnectionStringTest.Data;
using System;
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
        private readonly IApplicationDataService _applicationDataService;

        public ConnectionStringTester(IApplicationDataService applicationDataService)
        {
            _applicationDataService = applicationDataService;
        }

        public async Task<TestResponse> Test(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    SaveNewConnectionString(connectionString);
                    return new TestResponse(true, "Connected successfully.");
                }
            }
            catch (SqlException e)
            {
                SaveNewConnectionString(connectionString);
                return new TestResponse(false, e.Message);
            }
            catch (Exception e)
            {
                return new TestResponse(false, e.Message);
            }
        }

        private void SaveNewConnectionString(string connectionString)
        {
            var applicationData = _applicationDataService.GetApplicationData();
            if (applicationData.History.Contains(connectionString))
            {
                return;
            }

            var newApplicationData = new ApplicationData(applicationData.History.Append(connectionString));
            _applicationDataService.UpdateApplicationData(newApplicationData);
        }
    }
}
