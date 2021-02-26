using FluentAssertions;
using Rokt.Application;
using Rokt.Application.Interfaces;
using System;
using Xunit;

namespace Rokt.Tests
{
    public class RecordServiceTest
    {
        private readonly IRecordService _recordService;
        public RecordServiceTest()
        {
            _recordService = new RecordService();
        }

        [Fact]
        public void RecordService_Empty_Line()
        {
            var result = _recordService.ExtractLineFeed(string.Empty, ',', DateTime.Now, DateTime.Now);
            result.Should().BeNull();
        }

        [Fact]
        public void RecordService_Empty_Separator()
        {
            var result = _recordService.ExtractLineFeed("", default, DateTime.Now, DateTime.Now);
            result.Should().BeNull();
        }

        [Fact]
        public void RecordService_Valid_Line()
        {
            var result = _recordService.ExtractLineFeed("1990-01-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55", ' ', new DateTime(1990,01,01,0,0,0), new DateTime(1990, 01, 01, 18, 0, 0));
            result.Should().NotBeNull();
        }

        [Fact]
        public void RecordService_Valid_Line_NotMatchDate()
        {
            var result = _recordService.ExtractLineFeed("1990-01-01T02:29:00Z kelsi_brekke@schuppe.biz 21728403-8671-429f-99bd-404936312a55", ' ', new DateTime(1990, 01, 01, 0, 0, 0), new DateTime(1990, 01, 01, 04, 0, 0));
            result.Should().BeNull();
        }
    }
}
