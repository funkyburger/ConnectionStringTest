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
    public class TestButtonClickHandler : IEventHandler
    {
        public MainForm MainForm { get; private set; }

        public TestButtonClickHandler(MainForm mainForm)
        {
            MainForm = mainForm;
        }

        public async Task Handle(Event uievent, UserControl sender)
        {
            if(uievent != Event.TestButtonClicked)
            {
                return;
            }

            var mainTestControl = sender as MainTestControl;
             await Task.Run(() => {
                var task = ConnectionStringTester.Test(mainTestControl.ConnectionString);
                while (!task.IsCompleted)
                {
                }

                var result = task.Result;

                mainTestControl.DisplayMessage(result.Message, result.Success);
            });
        }
    }
}
