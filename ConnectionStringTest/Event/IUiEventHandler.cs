using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.Event
{
    public interface IUiEventHandler
    {
        void Handle(UiEvent uievent, UserControl sender);
    }
}
