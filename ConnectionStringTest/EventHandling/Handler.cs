﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.EventHandling
{
    public interface Handler
    {
        Task Handle(Event uievent, UserControl sender);
    }
}
