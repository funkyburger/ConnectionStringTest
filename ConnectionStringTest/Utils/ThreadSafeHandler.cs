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
    }
}
