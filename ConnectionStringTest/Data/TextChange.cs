using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class TextChange
    {
        public int Beginning { get; set; }
        public int End { get; set; }
        public string Modification { get; set; }
        public bool ReplaceAll => Beginning == 0 && End < 0;
    }
}
