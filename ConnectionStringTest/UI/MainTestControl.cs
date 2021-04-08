using ConnectionStringTest.Data;
using ConnectionStringTest.EventHandling;
using ConnectionStringTest.Exceptions;
using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public partial class MainTestControl : UserControl, IMainTestControl
    {
        private readonly IConnectionStringStore _connectionStringStore;
        private readonly IThreadSafeHandler _threadSafeHandler;

        public string ConnectionString => connectionStringBox.UnmaskedConnectionString;
        public string Message => testResultLabel.Message;

        public bool IsActionButtonEnabled
        {
            get
            {
                return actionButton.Enabled;
            }
            set
            {
                _threadSafeHandler.SetControlEnabled(actionButton, value);
            }
        }

        public MainTestControl(IConnectionStringStore connectionStringStore, 
            IThreadSafeHandler threadSafeHandler, 
            IConnectionStringTester connectionStringTester, 
            IConnectionStringCleaner connectionStringCleaner)
        {
            InitializeComponent();
            _connectionStringStore = connectionStringStore;
            _threadSafeHandler = threadSafeHandler;
            testResultLabel.Text = string.Empty;
            actionButton.Enabled = false;
            clipboardButton.Enabled = false;

            actionButton.AddEventHandler(new TestFiredHandler(connectionStringTester, connectionStringCleaner));

            clipboardButton.AddEventHandler(new MessageCopiedToClipboardHandler());

            connectionStringBox.AddEventHandler(new ConnectionStringBoxTextChangedHandler());

            RefreshAutoComplete();
        }

        public void DisplayMessage(string message, bool success = true)
        {
            testResultLabel.IsErrorMessage = !success;
            testResultLabel.Message = message;
        }

        public void SetStatus(TestStatus status)
        {
            if (status == TestStatus.Succeeded)
            {
                statusIcon.Image = Properties.Resources.statusIcon_success;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
                _threadSafeHandler.SetControlEnabled(clipboardButton, false);
                _threadSafeHandler.SetControlEnabled(connectionStringBox, true);
            }
            else if (status == TestStatus.Failed)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
                _threadSafeHandler.SetControlEnabled(clipboardButton, true);
                _threadSafeHandler.SetControlEnabled(connectionStringBox, true);
            }
            else if (status == TestStatus.Pending)
            {
                statusIcon.Image = Properties.Resources.statusIcon_loading;
                actionButton.CurrentAction = ActionButton.Action.Cancel;
                _threadSafeHandler.SetControlEnabled(clipboardButton, false);
                _threadSafeHandler.SetControlEnabled(connectionStringBox, false);
            }
            else if (status == TestStatus.Cancelled)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
                _threadSafeHandler.SetControlEnabled(clipboardButton, false);
                _threadSafeHandler.SetControlEnabled(connectionStringBox, true);
            }
            else
            {
                throw new Exception($"No icon found for status '{status}'.");
            }
        }

        public void RefreshAutoComplete()
        {
            connectionStringBox.RefreshAutoComplete();
        }

        public void UpdateTimer(TimeSpan elapsedTime)
        {
            _threadSafeHandler.WriteInLabel(timeLabel, $"{(int)elapsedTime.TotalSeconds}.{elapsedTime.Milliseconds:000}");
        }
    }
}
