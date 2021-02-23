using Rokt.Application.Interfaces;
using Rokt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rokt.Application
{
    public class DataService : IDataService
    {
        private readonly IFileService _fileService;

        public DataService(IFileService fileService)
        {
            _fileService = fileService;
        }
        public IEnumerable<LineFeed> ExtraData(string filePath, DateTime startDate, DateTime endDate)
        {
            var lineFeeds = _fileService.ReadFile(filePath, startDate, endDate);
            if (lineFeeds != null && lineFeeds.Any())
            {
                return lineFeeds.OrderBy(x => x.EventTime);
            }
            return lineFeeds;
        }
    }
}
