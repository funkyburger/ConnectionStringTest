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
        private readonly IPasswordMasker _passwordMasker;

        
        private readonly Dictionary<string, string> connectionStringsIndex = new Dictionary<string, string>();

        public ConnectionStringStore(IApplicationDataService applicationDataService, IPasswordMasker passwordMasker)
        {
            _applicationDataService = applicationDataService;
            _passwordMasker = passwordMasker;
        }

        public IEnumerable<string> GetConnectionStrings()
        {
            var data = _applicationDataService.GetApplicationData();

            foreach(var connectionString in data.History)
            {
                var maskedConnectionString = _passwordMasker.Mask(connectionString);

                if (maskedConnectionString == connectionString)
                {
                    continue;
                }

                while (connectionStringsIndex.ContainsKey(maskedConnectionString))
                {
                    maskedConnectionString = _passwordMasker.Mask(maskedConnectionString);
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
