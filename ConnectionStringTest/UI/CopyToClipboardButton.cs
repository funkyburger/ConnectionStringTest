using ConnectionStringTest.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public class CopyToClipboardButton : BaseButton
    {
        public CopyToClipboardButton()
        {
            BackgroundImageLayout = ImageLayout.Stretch;
            BackgroundImage = Properties.Resources.copyToClipboard;
            Location = new System.Drawing.Point(682, 29);
            Name = "copyToClipboardButton1";
            Size = new System.Drawing.Size(22, 22);
            TabIndex = 7;
            UseVisualStyleBackColor = true;
        }

        protected override async Task FireEvent()
        {
            foreach (var handler in Handlers)
            {
                await handler.Handle(Event.MessageCopiedToClipboard, this);
            }
        }
    }
}
