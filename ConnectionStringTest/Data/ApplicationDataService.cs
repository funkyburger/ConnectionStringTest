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

        private ApplicationData cachedApplicationData = null;
        private ApplicationData CachedApplicationData
        {
            get
            {
                if(cachedApplicationData == null)
                {
                    var content = _fileService.LoadApplicationDataFileContent();
                    cachedApplicationData = _serializer.Deserialize(content);
                }

                return cachedApplicationData;
            }
        }

        public ApplicationDataService(IFileService fileService, IApplicationDataSerializer serializer)
        {
            _fileService = fileService;
            _serializer = serializer;
        }

        public ApplicationData GetApplicationData()
        {
            return CachedApplicationData;
        }

        public void UpdateApplicationData(ApplicationData data)
        {
            // Invalidate cache
            cachedApplicationData = null;

            var content = _serializer.Serialize(data);

            _fileService.SaveApplicationDataFileContent(content);
        }
    }
}
