using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class StringHistoryStack : IEnumerable<HistoryStackItem>
    {
        private readonly IList<HistoryStackItem> stack = new List<HistoryStackItem>();
        private int index = 0;

        public HistoryStackItem Current
        {
            get { 
                return stack[index]; 
            }
        }

        public StringHistoryStack()
            : this(string.Empty, 0, 0)
        {
        }

        public StringHistoryStack(string first, int selectionStart, int selectionLength)
        {
            stack.Add(new HistoryStackItem(first, selectionStart, selectionLength));
        }

        public void Stack(string str, int selectionStart, int selectionLength)
        {
            if(str != Current.Value)
            {
                for(int i = stack.Count - 1; i > index; i--)
                {
                    stack.RemoveAt(i);
                }

                stack.Add(new HistoryStackItem(str, selectionStart, selectionLength));
                index++;
            }
        }

        public HistoryStackItem Undo()
        {
            if(index > 0)
            {
                index--;
            }

            return Current;
        }

        public HistoryStackItem Redo()
        {
            if(index < stack.Count - 1)
            {
                index++;
            }

            return Current;
        }

        public IEnumerator<HistoryStackItem> GetEnumerator()
        {
            return stack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return stack.GetEnumerator();
        }
    }
}
