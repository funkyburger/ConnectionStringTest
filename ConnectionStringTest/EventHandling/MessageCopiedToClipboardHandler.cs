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
        public async Task Handle(Event uievent, object sender)
        {
            if (uievent == Event.MessageCopiedToClipboard)
            {
                await CopyToClipboard((sender as CopyToClipboardButton).MainTestControl);
            }
            else
            {
                throw new UnhandledEnumException(uievent);
            }
        }

        private async Task CopyToClipboard(IMainTestControl mainTestControl)
        {
            Clipboard.SetText(mainTestControl.Message);
        }
    }
}
