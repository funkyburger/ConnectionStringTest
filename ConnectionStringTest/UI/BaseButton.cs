using ConnectionStringTest.EventHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public abstract class BaseButton : Button
    {
        protected readonly IList<IEventHandler> Handlers;

        public IMainTestControl MainTestControl { get; set; }

        protected BaseButton()
        {
            Click += OnClick;

            Handlers = new List<IEventHandler>();
        }

        public void AddEventHandler(IEventHandler handler)
        {
            Handlers.Add(handler);
        }

        private async void OnClick(object sender, EventArgs e)
        {
            await FireEvent();
        }

        protected abstract Task FireEvent();
    }
}
