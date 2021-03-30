using ConnectionStringTest.Timers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConnectionStringTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectionStringTestButton_Click(object sender, EventArgs e)
        {
            var result = ConnectionStringTestingService.Test(this.connectionStringBox.Text);

            ShowResult(result);
        }

        private void ShowResult(TestResponse response)
        {
            var color = response.Success ? Color.Black : Color.Red;

            testResultLabel.ForeColor = color;
            testResultLabel.Text = response.Message;
        }

        private void testResultLabel_Click(object sender, EventArgs e)
        {
            testResultLabel.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (testResultLabel.ContainsFocus)
            {
                Clipboard.SetText(testResultLabel.Text);
            }
        }

        private void ShowCopiedTextLabel(object sender, EventArgs e)
        {
            copiedAlertLabel.Visible = true;
            copiedAlertLabel.ForeColor = Color.Green;
            copiedAlertLabel.Text = "Copied!";
            var t = new DelayTimer();
            t.Interval = 5000; // it will Tick in 5 seconds
            t.Elapsed += this.HideInFiveSeconds;

            t.Start();
      
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This is under development
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void HideInFiveSeconds(object sender, EventArgs e)
        {
            int fadingSpeed = 3;
            copiedAlertLabel.ForeColor = Color.FromArgb(copiedAlertLabel.ForeColor.R + fadingSpeed, copiedAlertLabel.ForeColor.G + fadingSpeed, copiedAlertLabel.ForeColor.B + fadingSpeed);

            if (copiedAlertLabel.ForeColor.R >= this.BackColor.R)
            {
                var delayTimer = (DelayTimer)sender;
                delayTimer.Stop();
                copiedAlertLabel.ForeColor = this.BackColor;
            }
        }
    }
}