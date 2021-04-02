using ConnectionStringTest.Data;
using ConnectionStringTest.Exceptions;
using ConnectionStringTest.UI;
using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.EventHandling
{
    public class TestButtonClickHandler : IEventHandler
    {
        private readonly IConnectionStringTester _connectionStringTester;
        private readonly IConnectionStringCleaner _connectionStringCleaner;

        private CancellationTokenSource cancelationTokenSource;
        
        public bool TestPending { get; private set; }

        public TestButtonClickHandler(IConnectionStringTester connectionStringTester, IConnectionStringCleaner connectionStringCleaner)
        {
            _connectionStringTester = connectionStringTester;
            _connectionStringCleaner = connectionStringCleaner;
            TestPending = false;
        }

        public async Task Handle(Event uievent, object sender)
        {
            if(uievent == Event.TestButtonClicked)
            {
                await FireTest(sender as IMainTestControl);
            }
            else if (uievent == Event.TestCancelled)
            {
                await CancelTest();
            }
            else
            {
                throw new UnhandledEnumException(uievent);
            }

            if (uievent != Event.TestButtonClicked)
            {
                return;
            }
        }

        private async Task FireTest(IMainTestControl mainTestControl)
        {
            TestPending = true;
            cancelationTokenSource = new CancellationTokenSource();
            mainTestControl.SetStatus(TestStatus.Pending);

            await Task.Run(() =>
            {
                var start = DateTime.Now;
                mainTestControl.UpdateTimer(TimeSpan.Zero);
                var task = _connectionStringTester.Test(
                    _connectionStringCleaner.Clean(mainTestControl.ConnectionString));
                while (!task.IsCompleted)
                {
                    if (cancelationTokenSource.Token.IsCancellationRequested)
                    {
                        mainTestControl.SetStatus(TestStatus.Cancelled);

                        mainTestControl.DisplayMessage("Cancelled", false);
                        return;
                    }
                    mainTestControl.UpdateTimer(DateTime.Now - start);
                }

                mainTestControl.UpdateTimer(DateTime.Now - start);
                var result = task.Result;

                mainTestControl.SetStatus(result.Success ? TestStatus.Succeeded : TestStatus.Failed);

                mainTestControl.DisplayMessage(result.Message, result.Success);
            });

            mainTestControl.RefreshAutoComplete();
            TestPending = false;
        }

        private async Task CancelTest()
        {
            cancelationTokenSource.Cancel();
            TestPending = false;
        }
    }
}
