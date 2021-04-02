using ConnectionStringTest.Data;
using ConnectionStringTest.EventHandling;
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
    public partial class MainTestControl : UserControl
    {
        private readonly IApplicationDataService _applicationDataService;

        private readonly IList<IEventHandler> handlers;

        public string ConnectionString => connectionStringBox.Text;

        public MainTestControl(IApplicationDataService applicationDataService)
        {
            InitializeComponent();
            _applicationDataService = applicationDataService;
            handlers = new List<IEventHandler>();
            testResultLabel.Text = string.Empty;
            fireTestButton.Enabled = false;

            connectionStringBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            connectionStringBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            RefreshAutoComplete();
        }

        public void DisplayMessage(string message, bool success = true)
        {
            var color = success ? Color.Black : Color.Red;

            testResultLabel.ForeColor = color;
            testResultLabel.Text = message;
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
                fireTestButton.Enabled = true;
            }
            else if (status == TestStatus.Failed)
            {
                statusIcon.Image = Properties.Resources.statusIcon_failure;
                fireTestButton.Enabled = true;
            }
            else if (status == TestStatus.Pending)
            {
                statusIcon.Image = Properties.Resources.statusIcon_loading;
                fireTestButton.Enabled = false;
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
            timeLabel.Text = $"{(int)elapsedTime.TotalSeconds}.{elapsedTime.Milliseconds.ToString("000")}";
        }

        private async void fireTestButton_Click(object sender, EventArgs e)
        {
            foreach(var handler in handlers)
            {
                await handler.Handle(Event.TestButtonClicked, this);
            }
        }

        private void connectionStringBox_TextChanged(object sender, EventArgs e)
        {
            fireTestButton.Enabled = !string.IsNullOrEmpty(connectionStringBox.Text);
        }
    }
}
