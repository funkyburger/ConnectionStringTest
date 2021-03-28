﻿using Ninject.Modules;
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
            Bind(typeof(Utils.IConnectionStringCleaner)).To(typeof(Utils.ConnectionStringCleaner));

            Bind(typeof(Data.IFileService)).To(typeof(Data.FileService));
            Bind(typeof(Data.IApplicationDataService)).To(typeof(Data.ApplicationDataService));
            Bind(typeof(Data.IApplicationDataSerializer)).To(typeof(Data.ApplicationDataSerializer));

        }
    }
}
