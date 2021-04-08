using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class HistoryService : IHistoryService
    {
        private readonly IApplicationDataService _applicationDataService;
        private readonly IPasswordHelper _passwordHelper;

        public HistoryService(IApplicationDataService applicationDataService, IPasswordHelper passwordHelper)
        {
            _applicationDataService = applicationDataService;
            _passwordHelper = passwordHelper;
        }

        public IAutoCompleteStringWithPasswordsCollection GetAutoComplete()
        {
            var collection = new AutoCompleteStringWithPasswordsCollection();

            var connectionStrings = _applicationDataService.GetApplicationData().History;

            foreach(var connectionString in connectionStrings)
            {
                var masked = _passwordHelper.Mask(connectionString);

                while(!collection.TryInsert(masked, connectionString))
                {
                    masked = _passwordHelper.AddGarble(masked);
                }
            }

            return collection;
        }
    }
}
