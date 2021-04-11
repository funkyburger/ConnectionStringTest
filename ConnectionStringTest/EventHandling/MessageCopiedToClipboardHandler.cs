using ConnectionStringTest.Exceptions;
using ConnectionStringTest.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.EventHandling
{
    public class MessageCopiedToClipboardHandler : IEventHandler
    {
        public void Handle(Event uievent, object sender)
        {
            if (uievent == Event.MessageCopiedToClipboard)
            {
                CopyToClipboard((sender as CopyToClipboardButton).Parent as IMainTestControl);
            }
            else
            {
                throw new UnhandledEnumException(uievent);
            }
        }

        private void CopyToClipboard(IMainTestControl mainTestControl)
        {
            Clipboard.SetText(mainTestControl.Message);
        }
    }
}
