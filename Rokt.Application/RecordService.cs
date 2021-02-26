using Rokt.Application.Interfaces;
using Rokt.Domain;
using System;
using System.Linq;

namespace Rokt.Application
{
    public class RecordService : IRecordService
    {
        public LineFeed ExtractLineFeed(string line, char separator, DateTime startDate, DateTime enDate)
        {
            if (string.IsNullOrEmpty(line))
            {
                return default;
            }
            string[] feedArray = line.Split(separator);
            if (feedArray == null || !feedArray.Any())
            {
                return default;
            }
            if (!DateTime.TryParse(feedArray[0], out DateTime _lineDate))
            {
                return default;
            }
            if (_lineDate >= startDate && _lineDate <= enDate)
            {
                return new LineFeed
                {
                    EventTime = _lineDate,
                    Email = feedArray[1],
                    SessionId = feedArray[2]
                };
            }
            return default;
        }
    }
}