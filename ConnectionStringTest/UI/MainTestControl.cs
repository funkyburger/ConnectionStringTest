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
        private readonly IApplicationDataService _applicationDataService;
        private readonly IThreadSafeHandler _threadSafeHandler;

        private readonly IList<IEventHandler> handlers;

        public string ConnectionString => connectionStringBox.Text;

        public MainTestControl(IApplicationDataService applicationDataService, IThreadSafeHandler threadSafeHandler)
        {
            InitializeComponent();
            _applicationDataService = applicationDataService;
            _threadSafeHandler = threadSafeHandler;
            handlers = new List<IEventHandler>();
            testResultLabel.Text = string.Empty;
            actionButton.Enabled = false;

            connectionStringBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            connectionStringBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            RefreshAutoComplete();
        }

        public void DisplayMessage(string message, bool success = true)
        {
            var color = success ? Color.Black : Color.Red;

            testResultLabel.ForeColor = color;
            _threadSafeHandler.WriteInLabel(testResultLabel, message);
        }

        public void AddHandler(IEventHandler handler)
        {
            handlers.Add(handler);
        }

        public void SetStatus(TestStatus status)
        {
            if(status == TestStatus.Succeeded)
            {
                statusIcon.Image = Properties.Resources.statusIcon_success;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
            }
            else if (status == TestStatus.Failed)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
            }
            else if (status == TestStatus.Pending)
            {
                statusIcon.Image = Properties.Resources.statusIcon_loading;
                actionButton.CurrentAction = ActionButton.Action.Cancel;
            }
            else if (status == TestStatus.Cancelled)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                actionButton.CurrentAction = ActionButton.Action.FireTest;
            }
            else
            {
                throw new Exception($"No icon found for status '{status}'.");
            }
        }

        public void RefreshAutoComplete()
        {
            connectionStringBox.AutoCompleteCustomSource.Clear();
            connectionStringBox.AutoCompleteCustomSource.AddRange(_applicationDataService.GetApplicationData()
                .History.ToArray());
        }

        public void UpdateTimer(TimeSpan elapsedTime)
        {
            _threadSafeHandler.WriteInLabel(timeLabel, $"{(int)elapsedTime.TotalSeconds}.{elapsedTime.Milliseconds:000}");
        }

        private void connectionStringBox_TextChanged(object sender, EventArgs e)
        {
            actionButton.Enabled = !string.IsNullOrEmpty(connectionStringBox.Text);
        }

        private async void actionButtonClicked(object sender, EventArgs e)
        {
            Event eVent;
            if(actionButton.CurrentAction == ActionButton.Action.FireTest)
            {
                eVent = Event.TestFired;
            }
            else if (actionButton.CurrentAction == ActionButton.Action.Cancel)
            {
                eVent = Event.TestCancelled;
            }
            else
            {
                throw new UnhandledEnumException(actionButton.CurrentAction);
            }

            foreach (var handler in handlers)
            {
                await handler.Handle(eVent, this);
            }
        }
    }
}
