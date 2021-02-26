using Newtonsoft.Json;
using Rokt.Application.Interfaces;
using Rokt.Domain.Requests;
using System.Linq;

namespace Rokt.Application
{
    public class DataService : IDataService
    {
        private readonly IFileService _fileService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public DataService(IFileService fileService, JsonSerializerSettings jsonSerializerSettings)
        {
            _fileService = fileService;
            _jsonSerializerSettings = jsonSerializerSettings;
        }

        public string ExtractData(EventSearchRequest eventSearchRequest)
        {
            var lineFeeds = _fileService.Process(eventSearchRequest);
            if (lineFeeds != null && lineFeeds.Any())
            {
                return JsonConvert.SerializeObject(lineFeeds.OrderBy(x => x.EventTime), _jsonSerializerSettings);
            }
            return string.Empty;
        }
    }
}