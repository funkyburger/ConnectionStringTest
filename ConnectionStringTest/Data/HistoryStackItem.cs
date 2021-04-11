using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class HistoryStackItem : IEquatable<HistoryStackItem>
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

        public override int GetHashCode()
        {
            return $"{Value};{SelectionStart};{SelectionLength}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as HistoryStackItem;
            if (other == null)
            {
                return false;
            }

            return ValueEquals(other);
        }

        public bool Equals(HistoryStackItem other)
        {
            if (other == null)
            {
                return false;
            }

            return ValueEquals(other);
        }

        public static bool operator == (HistoryStackItem obj1, HistoryStackItem obj2)
        {
            if (ReferenceEquals(obj1, null) && ReferenceEquals(obj2, null))
            {
                return true;
            }
            else if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
            {
                return false;
            }

            //System.Diagnostics.Debug.WriteLine("pouet");

            //return obj1.Equals(obj2);
            return obj1.Value == obj2.Value
                && obj1.SelectionStart == obj2.SelectionStart
                && obj1.SelectionLength == obj2.SelectionLength;
        }

        // this is second one '!='
        public static bool operator != (HistoryStackItem obj1, HistoryStackItem obj2)
        {
            return !(obj1 == obj2);
        }

        public override string ToString()
        {
            return $"'{Value}', {SelectionStart}, {SelectionLength}" ;
        }

        private bool ValueEquals(HistoryStackItem other)
        {
            return Value == other.Value
                && SelectionStart == other.SelectionStart
                && SelectionLength == other.SelectionLength;
        }
    }
}
