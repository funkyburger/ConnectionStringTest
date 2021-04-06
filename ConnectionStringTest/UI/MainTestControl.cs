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

        public string ConnectionString => _connectionStringStore.GetConnectionStringWithPassword(connectionStringBox.Text);
        public string Message => testResultLabel.Message;

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

            connectionStringBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            connectionStringBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            actionButton.AddEventHandler(new TestFiredHandler(connectionStringTester, connectionStringCleaner));
            actionButton.MainTestControl = this;

            clipboardButton.AddEventHandler(new MessageCopiedToClipboardHandler());
            clipboardButton.MainTestControl = this;

            RefreshAutoComplete();
        }

        public void DisplayMessage(string message, bool success = true)
        {
            testResultLabel.IsErrorMessage = !success;
            testResultLabel.Message = message;
        }

        public void SetStatus(TestStatus status)
        {
            if(status == TestStatus.Succeeded)
            {
                statusIcon.Image = Properties.Resources.statusIcon_success;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
                _threadSafeHandler.SetButtonEnabled(clipboardButton, false);
            }
            else if (status == TestStatus.Failed)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
                _threadSafeHandler.SetButtonEnabled(clipboardButton, true);
            }
            else if (status == TestStatus.Pending)
            {
                statusIcon.Image = Properties.Resources.statusIcon_loading;
                actionButton.CurrentAction = ActionButton.Action.Cancel;
                _threadSafeHandler.SetButtonEnabled(clipboardButton, false);
            }
            else if (status == TestStatus.Cancelled)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
                _threadSafeHandler.SetButtonEnabled(clipboardButton, false);
            }
            else
            {
                throw new Exception($"No icon found for status '{status}'.");
            }
        }

        public void RefreshAutoComplete()
        {
            connectionStringBox.AutoCompleteCustomSource.Clear();
            connectionStringBox.AutoCompleteCustomSource.AddRange(_connectionStringStore.GetConnectionStrings().ToArray());
        }

        public void UpdateTimer(TimeSpan elapsedTime)
        {
            _threadSafeHandler.WriteInLabel(timeLabel, $"{(int)elapsedTime.TotalSeconds}.{elapsedTime.Milliseconds:000}");
        }

        private void connectionStringBox_TextChanged(object sender, EventArgs e)
        {
            actionButton.Enabled = !string.IsNullOrEmpty(connectionStringBox.Text);
        }
    }
}
