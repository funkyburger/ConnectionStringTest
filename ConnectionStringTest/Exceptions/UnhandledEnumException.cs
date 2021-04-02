using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Exceptions
{
    public class UnhandledEnumException : Exception
    {
        public Enum Enum { get; private set; }

        public UnhandledEnumException(Enum @enum)
            : base($"Enum '{@enum}' of type '{@enum.GetType().FullName}' could not be handled.")
        {
            Enum = @enum;
        }
    }
}
