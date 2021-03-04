using ConnectionStringTest.Event;
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
        private IList<IUiEventHandler> uiEventHandlers;

        public string ConnectionString => connectionStringBox.Text;

        public MainTestControl()
        {
            InitializeComponent();
            uiEventHandlers = new List<IUiEventHandler>();
            testResultLabel.Text = string.Empty;
        }

        public void DisplayMessage(string message, bool success = true)
        {
            var color = success ? Color.Black : Color.Red;

            testResultLabel.ForeColor = color;
            testResultLabel.Text = message;
        }

        public void AddUiEventHandler(IUiEventHandler handler)
        {
            uiEventHandlers.Add(handler);
        }

        private void fireTestButton_Click(object sender, EventArgs e)
        {
            foreach(var handler in uiEventHandlers)
            {
                handler.Handle(UiEvent.TestButtonClicked, this);
            }
        }
    }
}
