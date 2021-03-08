using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest
{
    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(Utils.IConnectionStringTester)).To(typeof(Utils.ConnectionStringTester));
        }
    }
}
