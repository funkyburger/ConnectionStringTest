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
    public class TestFiredHandler : IEventHandler
    {
        private readonly IConnectionStringTester _connectionStringTester;
        private readonly IConnectionStringCleaner _connectionStringCleaner;

        private CancellationTokenSource cancelationTokenSource;
        
        public bool TestPending { get; private set; }

        public TestFiredHandler(IConnectionStringTester connectionStringTester, IConnectionStringCleaner connectionStringCleaner)
        {
            _connectionStringTester = connectionStringTester;
            _connectionStringCleaner = connectionStringCleaner;
            TestPending = false;
        }

        public async Task Handle(Event uievent, object sender)
        {
            if(uievent == Event.TestFired)
            {
                await FireTest((sender as ActionButton).MainTestControl);
            }
            else if (uievent == Event.TestCancelled)
            {
                await CancelTest();
            }
            else
            {
                throw new UnhandledEnumException(uievent);
            }
        }

        // Intended to make async test possible.
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task FireTest(IMainTestControl mainTestControl)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (TestPending)
            {
                throw new Exception("A test is already running.");
            }

            TestPending = true;
            cancelationTokenSource = new CancellationTokenSource();
            mainTestControl.SetStatus(TestStatus.Pending);

            // This is intended, it should run in background (and is the whole point of making this call chain async)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(() =>
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
                        TestPending = false;
                        return;
                    }
                    mainTestControl.UpdateTimer(DateTime.Now - start);
                }

                mainTestControl.UpdateTimer(DateTime.Now - start);
                var result = task.Result;

                mainTestControl.SetStatus(result.Success ? TestStatus.Succeeded : TestStatus.Failed);

                mainTestControl.DisplayMessage(result.Message, result.Success);
                TestPending = false;
            });
#pragma warning restore CS4014

            mainTestControl.RefreshAutoComplete();
        }

        private async Task CancelTest()
        {
            cancelationTokenSource.Cancel();
            TestPending = false;
        }
    }
}
