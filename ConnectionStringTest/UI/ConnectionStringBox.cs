using ConnectionStringTest.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public class ConnectionStringBox : TextBox
    {
        protected readonly IList<IEventHandler> Handlers;

        public IMainTestControl MainTestControl { get; set; }

        public ConnectionStringBox()
        {
            Handlers = new List<IEventHandler>();
            TextChanged += OnTextChanged;
        }

        public void AddEventHandler(IEventHandler handler)
        {
            Handlers.Add(handler);
        }

        private async void OnTextChanged(object sender, EventArgs e)
        {
            foreach (var handler in Handlers)
            {
                await handler.Handle(Event.ConnectionStringBoxTextChanged, this);
            }
        }
    }
}
