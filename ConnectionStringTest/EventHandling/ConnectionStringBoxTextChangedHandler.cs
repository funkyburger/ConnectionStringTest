﻿using ConnectionStringTest.Exceptions;
using ConnectionStringTest.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.EventHandling
{
    public class ConnectionStringBoxTextChangedHandler : IEventHandler
    {
        public Task Handle(Event uievent, object sender)
        {
            if(uievent != Event.ConnectionStringBoxTextChanged)
            {
                throw new UnhandledEnumException(uievent);
            }

            var connectionStringBox = sender as ConnectionStringBox;
            
            Debug.WriteLine($"New text : '{connectionStringBox.Text}'");

            connectionStringBox.MainTestControl.IsActionButtonEnabled = !string.IsNullOrEmpty(connectionStringBox.Text);

            return Task.CompletedTask;
        }
    }
}
