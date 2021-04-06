using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.Utils
{
    public class ThreadSafeHandler : IThreadSafeHandler
    {
        private delegate void SafeLabelWriteCallDelegate(Label label, string text);
        private delegate void SafeSetButtonEnbled(Button button, bool enabled);

        public void WriteInLabel(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                var callDelegate = new SafeLabelWriteCallDelegate(WriteInLabel);
                label.Invoke(callDelegate, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
        }

        public void SetButtonEnabled(Button button, bool enabled)
        {
            if (button.InvokeRequired)
            {
                var callDelegate = new SafeSetButtonEnbled(SetButtonEnabled);
                button.Invoke(callDelegate, new object[] { button, enabled });
            }
            else
            {
                button.Enabled = enabled;
            }
        }
    }
}
