using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Rokt.Application;
using Rokt.Application.Interfaces;
using Rokt.Domain.Requests;
using System;
using Xunit;

namespace Rokt.Tests
{
    public class DataServiceTest : BaseTest
    {
        private readonly IDataService _dataService;
        private readonly IFileService _fileService;
        private readonly IValidationService _validationService;
        private readonly IRecordService _recordService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        public DataServiceTest()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            _validationService = new ValidationService();
            _recordService = new RecordService();
            _fileService = new FileService(_validationService, _recordService);
            _dataService = new DataService(_fileService, _jsonSerializerSettings);
        }

        [Fact]

        public void DataService_ProcessRequest_NotRange()
        {
            //arrange
            string eventFeeds = @"1990-02-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55
1990-02-01T04:26:59Z martina@daugherty.ca 6ca02e3d-e978-4f2f-b920-a6fd01a275fc
1990-02-01T10:44:59Z arnoldo.treutel@bartoletti.info 48b9200c-e81c-4eb3-a044-69b2b484c09d
1990-02-01T10:45:32Z walker_mayert@kreigergrady.biz 25ba55dd-0060-4004-a776-ef5ac6d29b84";

            var fileInfo = CreateTestFile(eventFeeds);

            var request = new EventSearchRequest
            {
                FilePath = fileInfo.FullName,
                StartDate = new DateTime(1990, 01, 01, 0, 0, 0),
                EndDate = new DateTime(1990, 01, 01, 23, 59, 59),
                FeedSeparator = ' '
            };

            //act            
            var result = _dataService.ExtractData(request);
            result.Should().BeEmpty();
        }

        [Fact]

        public void DataService_ProcessRequest()
        {
            //arrange
            string eventFeeds = @"1990-01-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55
1990-01-01T04:26:59Z martina@daugherty.ca 6ca02e3d-e978-4f2f-b920-a6fd01a275fc
1990-01-01T10:44:59Z arnoldo.treutel@bartoletti.info 48b9200c-e81c-4eb3-a044-69b2b484c09d
1990-01-01T10:45:32Z walker_mayert@kreigergrady.biz 25ba55dd-0060-4004-a776-ef5ac6d29b84";

            var fileInfo = CreateTestFile(eventFeeds);

            var request = new EventSearchRequest
            {
                FilePath = fileInfo.FullName,
                StartDate = new DateTime(1990, 01, 01, 0, 0, 0),
                EndDate = new DateTime(1990, 01, 01, 14, 59, 59),
                FeedSeparator = ' '
            };

            //act            
            var result = _dataService.ExtractData(request);
            result.Should().NotBeNull();
            var actual = JToken.Parse(result);
            var expected = JToken.Parse(@"[
            {
              ""email"": ""kelsi_brekke@schuppe.biz"",
              ""sessionId"": ""21728403-8671-429f-99bd-404936312a55"",
              ""eventTime"": ""1990-01-01T02:29:00Z""
            }]");
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
