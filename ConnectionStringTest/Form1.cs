using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void testbutton_Click(object sender, EventArgs e)
        {
            var result = ConnectionStringTester.Test(this.connectionStringBox.Text);

            ShowResult(result);
        }

        private void ShowResult(TestResponse response)
        {
            var color = response.Success ? Color.Black : Color.Red;

            testResultLabel.ForeColor = color;
            testResultLabel.Text = response.Message;
        }
    }
}
