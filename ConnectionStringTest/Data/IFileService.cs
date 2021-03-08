using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public interface IFileService
    {
        string LoadApplicationDataFileContent();

        void SaveApplicationDataFileContent(string content);
    }
}
