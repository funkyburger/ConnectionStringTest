using ConnectionStringTest.EventHandling;
using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public class ConnectionStringBox : PasswordTextBox, ILinkedToMainTestControl
    {
        public IMainTestControl MainTestControl => Parent as IMainTestControl;

        public ConnectionStringBox(IPasswordHelper passwordHelper)
            : base(passwordHelper)
        {
        }
    }
}
