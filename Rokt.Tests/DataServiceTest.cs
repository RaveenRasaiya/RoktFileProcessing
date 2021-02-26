using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Rokt.Application;
using Rokt.Application.Interfaces;
using Rokt.Domain;
using Rokt.Domain.Requests;
using System;
using System.Collections.Generic;
using Xunit;

namespace Rokt.Tests
{
    public class DataServiceTest
    {
        private readonly IDataService _dataService;
        private readonly Mock<IFileService> _fileService;
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public DataServiceTest()
        {
            _jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            _fileService = new Mock<IFileService>();
            _dataService = new DataService(_fileService.Object, _jsonSerializerSettings);
        }

        [Fact]
        public void DataService_ProcessRequest()
        {
            //arrange

            _fileService.Setup(f => f.Process(It.IsAny<EventSearchRequest>())).Returns(new List<LineFeed>
            {
               new LineFeed
                    {
                        Email = "mock@gmail.com",
                        SessionId = Guid.NewGuid().ToString(),
                        EventTime = DateTime.Now
                    },
           });

            var request = new EventSearchRequest
            {
                FilePath = @"C:\filepath",
                StartDate = new DateTime(1990, 01, 01, 0, 0, 0),
                EndDate = new DateTime(1990, 01, 01, 14, 59, 59),
                FeedSeparator = ' '
            };

            //act
            var result = _dataService.ExtractData(request);
            result.Should().NotBeNullOrEmpty();
            var actual = JToken.Parse(result);
            var expected = JToken.Parse(@"[
            {
              ""email"": ""kelsi_brekke@schuppe.biz"",
              ""sessionId"": ""21728403-8671-429f-99bd-404936312a55"",
              ""eventTime"": ""1990-01-01T02:29:00Z""
            }]");
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void DataService_ProcessRequest_Emtpy_Result()
        {
            //arrange

            _fileService.Setup(f => f.Process(It.IsAny<EventSearchRequest>())).Returns(new List<LineFeed> { });

            var request = new EventSearchRequest
            {
                FilePath = @"C:\filepath",
                StartDate = new DateTime(1990, 01, 01, 0, 0, 0),
                EndDate = new DateTime(1990, 01, 01, 14, 59, 59),
                FeedSeparator = ' '
            };

            //act
            var result = _dataService.ExtractData(request);
            result.Should().BeEmpty();
        }
    }
}