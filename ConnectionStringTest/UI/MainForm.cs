using ConnectionStringTest.EventHandling;
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

namespace ConnectionStringTest.UI
{
    public partial class MainForm : Form
    {
        public MainForm(IConnectionStringTester connectionStringTester)
        {
            InitializeComponent();
            mainTestControl.AddHandler(new TestButtonClickHandler(connectionStringTester, this));
        }
    }
}
