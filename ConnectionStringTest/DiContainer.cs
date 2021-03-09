using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest
{
    public static class DiContainer
    {
        private static IKernel _ninjectKernel;

        public static void Wire(IKernel ninjectKernel)
        {
            _ninjectKernel = ninjectKernel;
        }

        public static T Resolve<T>()
        {
            return _ninjectKernel.Get<T>();
        }
    }
}
