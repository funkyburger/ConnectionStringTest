using ConnectionStringTest.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Utils
{
    public class ConnectionStringStore : IConnectionStringStore
    {
        private readonly IApplicationDataService _applicationDataService;
        private readonly IPasswordHelper _passwordHelper;

        
        private readonly Dictionary<string, string> connectionStringsIndex = new Dictionary<string, string>();

        public ConnectionStringStore(IApplicationDataService applicationDataService, IPasswordHelper passwordHelper)
        {
            _applicationDataService = applicationDataService;
            _passwordHelper = passwordHelper;
        }

        public IEnumerable<string> GetConnectionStrings()
        {
            var data = _applicationDataService.GetApplicationData();

            foreach(var connectionString in data.History)
            {
                var maskedConnectionString = _passwordHelper.Mask(connectionString);

                if (maskedConnectionString == connectionString)
                {
                    continue;
                }

                while (connectionStringsIndex.ContainsKey(maskedConnectionString))
                {
                    maskedConnectionString = _passwordHelper.Mask(maskedConnectionString);
                }

                connectionStringsIndex.Add(maskedConnectionString, connectionString);
            }

            return connectionStringsIndex.Keys;
        }

        public string GetConnectionStringWithPassword(string connectionString)
        {
            string result;
            if(connectionStringsIndex.TryGetValue(connectionString, out result))
            {
                return result;
            }

            return connectionString;
        }
    }
}
