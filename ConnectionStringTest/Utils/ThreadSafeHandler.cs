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
        private delegate void SafeSetControlEnabled(Control button, bool enabled);

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

        public void SetControlEnabled(Control control, bool enabled)
        {
            if (control.InvokeRequired)
            {
                var callDelegate = new SafeSetControlEnabled(SetControlEnabled);
                control.Invoke(callDelegate, new object[] { control, enabled });
            }
            else
            {
                control.Enabled = enabled;
            }
        }
    }
}
