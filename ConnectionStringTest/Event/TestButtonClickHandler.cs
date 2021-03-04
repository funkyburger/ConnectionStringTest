using ConnectionStringTest.UI;
using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.Event
{
    public class TestButtonClickHandler : IUiEventHandler
    {
        public MainForm MainForm { get; private set; }

        public TestButtonClickHandler(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public void Handle(UiEvent uievent, UserControl sender)
        {
            if(uievent != UiEvent.TestButtonClicked)
            {
                return;
            }

            var mainTestControl = sender as MainTestControl;
            var result = ConnectionStringTester.Test(mainTestControl.ConnectionString);

            mainTestControl.DisplayMessage(result.Message, result.Success);
        }
    }
}
