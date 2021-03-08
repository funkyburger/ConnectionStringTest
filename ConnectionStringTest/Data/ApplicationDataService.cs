using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.Data
{
    public class ApplicationDataService : IApplicationDataService
    {
        private readonly IFileService _fileService;
        private readonly IApplicationDataSerializer _serializer;

        public ApplicationDataService(IFileService fileService, IApplicationDataSerializer serializer)
        {
            _fileService = fileService;
            _serializer = serializer;
        }

        public ApplicationData GetApplicationData()
        {
            var content = _fileService.LoadApplicationDataFileContent();

            return _serializer.Deserialize(content);
        }

        public void UpdateApplicationData(ApplicationData data)
        {
            var content = _serializer.Serialize(data);

            _fileService.SaveApplicationDataFileContent(content);
        }
    }
}
