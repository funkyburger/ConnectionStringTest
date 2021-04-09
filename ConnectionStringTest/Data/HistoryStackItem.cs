using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class HistoryStackItem
    {
        public string Value { get; private set; }
        public int SelectionStart { get; private set; }
        public int SelectionLength { get; private set; }

        public HistoryStackItem(string value, int selectionStart, int selectionLength)
        {
            Value = value;
            SelectionStart = selectionStart;
            SelectionLength = selectionLength;
        }
    }
}
