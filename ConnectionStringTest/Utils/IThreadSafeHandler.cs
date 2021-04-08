using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.Utils
{
    public interface IThreadSafeHandler
    {
        void WriteInLabel(Label label, string text);
        void SetControlEnabled(Control control, bool enabled);
    }
}
