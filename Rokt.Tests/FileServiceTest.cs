using FluentAssertions;
using Rokt.Application;
using Rokt.Application.Interfaces;
using Rokt.Domain.Requests;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using Xunit;

namespace Rokt.Tests
{
    public class FileServiceTest : BaseTest
    {

        private readonly IFileService _fileService;
        private readonly IValidationService _validationService;
        private readonly IRecordService _recordService;
        public FileServiceTest()
        {
            _validationService = new ValidationService();
            _recordService = new RecordService();
            _fileService = new FileService(_validationService, _recordService);
        }


        [Fact]
        public void FileService_EventSearchRequest_Null()
        {
            Action act = () => _fileService.ReadFile(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void FileService_EventSearchRequest_FileNoFound()
        {
            var request = new EventSearchRequest
            {
                FilePath = @"C:\FilnotFoumnd",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                FeedSeparator = ' '
            };
            Action act = () => _fileService.ReadFile(request);
            act.Should().Throw<FileNotFoundException>();
        }

        [Fact]
        public void FileService_EventSearchRequest_EmptyFileName()
        {
            var fileInfo = CreateTestFile(string.Empty);

            var request = new EventSearchRequest
            {
                FilePath = string.Empty,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            };
            Action act = () => _fileService.ReadFile(request);
            act.Should().Throw<ValidationException>().And.Message.Should().Be("FilePath is empty.");
            fileInfo.Delete();
        }

        [Fact]
        public void FileService_EventSearchRequest_Not_Valid_Start_Date()
        {
            var fileInfo = CreateTestFile(string.Empty);

            var request = new EventSearchRequest
            {
                FilePath = fileInfo.FullName,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.Now,
            };
            Action act = () => _fileService.ReadFile(request);
            act.Should().Throw<ValidationException>().And.Message.Should().Be("StartDate is not valid.");
            fileInfo.Delete();
        }

        [Fact]
        public void FileService_EventSearchRequest_Not_Valid_Finish_Date()
        {
            var fileInfo = CreateTestFile(string.Empty);

            var request = new EventSearchRequest
            {
                FilePath = fileInfo.FullName,
                StartDate = DateTime.Now,
                EndDate = DateTime.MinValue,
                FeedSeparator = ' '
            };
            Action act = () => _fileService.ReadFile(request);
            act.Should().Throw<ValidationException>().And.Message.Should().Be("EndDate is not valid.");
            fileInfo.Delete();
        }


        [Fact]
        public void FileService_EventSearchRequest_Valid_File()
        {
            string eventFeeds = @"1990-01-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55
1990-01-01T04:26:59Z martina@daugherty.ca 6ca02e3d-e978-4f2f-b920-a6fd01a275fc
1990-01-01T10:44:59Z arnoldo.treutel@bartoletti.info 48b9200c-e81c-4eb3-a044-69b2b484c09d
1990-01-01T10:45:32Z walker_mayert@kreigergrady.biz 25ba55dd-0060-4004-a776-ef5ac6d29b84";

            var fileInfo = CreateTestFile(eventFeeds);

            var request = new EventSearchRequest
            {
                FilePath = fileInfo.FullName,
                StartDate = new DateTime(1990, 01, 01, 0, 0, 0),
                EndDate = new DateTime(1990, 01, 01, 23, 59, 59),
                FeedSeparator = ' '
            };
            var result = _fileService.ReadFile(request);
            result.Should().NotBeNull();
            result.Should().HaveCount(4);
            fileInfo.Delete();
        }

        [Fact]
        public void FileService_EventSearchRequest_Valid_File_WithMoreLines()
        {

            string eventFeeds = @"1990-01-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55
                                1990-01-01T04:26:59Z martina@daugherty.ca 6ca02e3d-e978-4f2f-b920-a6fd01a275fc
                                1990-01-01T10:44:59Z arnoldo.treutel@bartoletti.info 48b9200c-e81c-4eb3-a044-69b2b484c09d
                                1990-01-01T18:45:32Z walker_mayert@kreigergrady.biz 25ba55dd-0060-4004-a776-ef5ac6d29b84";

            var fileInfo = CreateTestFile(eventFeeds);
            var request = new EventSearchRequest
            {
                FilePath = fileInfo.FullName,
                StartDate = new DateTime(1990, 01, 01, 0, 0, 0),
                EndDate = new DateTime(1990, 01, 01, 14, 59, 59),
                FeedSeparator = ' '
            };
            var result = _fileService.ReadFile(request);
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            fileInfo.Delete();
        }
    }
}
