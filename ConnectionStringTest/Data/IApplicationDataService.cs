﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public interface IApplicationDataService
    {
        ApplicationData GetApplicationData();
        void UpdateApplicationData(ApplicationData data);
    }
}
