using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.EventHandling
{
    public interface IEventHandler
    {
        Task Handle(Event uievent, object sender);
    }
}
