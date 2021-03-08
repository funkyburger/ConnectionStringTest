using ConnectionStringTest.Data;
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
        private readonly IConnectionStringTester _connectionStringTester;

        public MainForm MainForm { get; private set; }

        public TestButtonClickHandler(IConnectionStringTester connectionStringTester, MainForm mainForm)
        {
            _connectionStringTester = connectionStringTester;
            MainForm = mainForm;
        }

        public async Task Handle(Event uievent, UserControl sender)
        {
            if (uievent != Event.TestButtonClicked)
            {
                return;
            }

            var mainTestControl = sender as MainTestControl;
            mainTestControl.SetStatus(TestStatus.Pending);

            await Task.Run(() =>
            {
                var start = DateTime.Now;
                mainTestControl.UpdateTimer(TimeSpan.Zero);
                var task = _connectionStringTester.Test(mainTestControl.ConnectionString);
                while (!task.IsCompleted)
                {
                    mainTestControl.UpdateTimer(DateTime.Now - start);
                }

                mainTestControl.UpdateTimer(DateTime.Now - start);
                var result = task.Result;

                mainTestControl.SetStatus(result.Success ? TestStatus.Succeeded : TestStatus.Failed);

                mainTestControl.DisplayMessage(result.Message, result.Success);
            });
        }
    }
}
