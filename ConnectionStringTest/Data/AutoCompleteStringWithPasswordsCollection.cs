using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.Data
{
    public class AutoCompleteStringWithPasswordsCollection : AutoCompleteStringCollection, IAutoCompleteStringWithPasswordsCollection
    {
        private IDictionary<string, string> UnmaskedRegister = new Dictionary<string, string>();

        public bool TryInsert(string masked, string unmasked)
        {
            if (UnmaskedRegister.ContainsKey(masked))
            {
                return false;
            }

            UnmaskedRegister.Add(masked, unmasked);
            Add(masked);

            return true;
        }

        public bool GetUnmasked(string connectionString, out string unmasked)
        {
            return UnmaskedRegister.TryGetValue(connectionString, out unmasked);
        }

        public string GetUnmasked(string connectionString)
        {
            string unmasked;
            if(UnmaskedRegister.TryGetValue(connectionString, out unmasked))
            {
                return unmasked;
            }

            throw new Exception($"Could not find unamsked of string '{connectionString}'.");
        }
    }
}
