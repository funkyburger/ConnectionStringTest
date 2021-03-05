using ConnectionStringTest.UI;
using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.EventHandling
{
    public class TestButtonClickHandler : Handler
    {
        public MainForm MainForm { get; private set; }

        public TestButtonClickHandler(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void Handle(Event uievent, UserControl sender)
        {
            if(uievent != Event.TestButtonClicked)
            {
                return;
            }

            var mainTestControl = sender as MainTestControl;
            var result = ConnectionStringTester.Test(mainTestControl.ConnectionString);

            mainTestControl.DisplayMessage(result.Message, result.Success);
        }
    }
}
