using ConnectionStringTest.Data;
using ConnectionStringTest.EventHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly ComponentResourceManager resourceManager = new ComponentResourceManager(typeof(MainTestControl));

        private readonly IList<IEventHandler> handlers;

        public string ConnectionString => connectionStringBox.Text;

        public MainTestControl()
        {
            InitializeComponent();
            handlers = new List<IEventHandler>();
            testResultLabel.Text = string.Empty;
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
                statusIcon.Image = (Image)resourceManager.GetObject("statusIcon.success");
            }
            else if (status == TestStatus.Failed)
            {
                statusIcon.Image = (Image)resourceManager.GetObject("statusIcon.failure");
            }
            else if (status == TestStatus.Pending)
            {
                statusIcon.Image = (Image)resourceManager.GetObject("statusIcon.loading");
            }
            else
            {
                throw new Exception($"No icon found for status '{status}'.");
            }
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
    }
}
