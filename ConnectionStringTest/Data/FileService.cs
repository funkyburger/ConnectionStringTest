using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class FileService : IFileService
    {
        private readonly string Filename = $"{Path.GetTempPath()}cst.tmp";

        public string LoadApplicationDataFileContent()
        {
            try
            {
                using (var stream = File.OpenRead(Filename))
                {
                    byte[] buffer = new byte[(int)stream.Length];
                    stream.Read(buffer, 0, (int)stream.Length);

                    return Encoding.ASCII.GetString(buffer);
                }
            }
            catch(FileNotFoundException)
            {
                return string.Empty;
            }
        }

        public void SaveApplicationDataFileContent(string content)
        {
            using (var stream = File.OpenWrite(Filename))
            {
                stream.Write(Encoding.ASCII.GetBytes(content), 0, content.Length);
            }

            File.Encrypt(Filename);
        }
    }
}
