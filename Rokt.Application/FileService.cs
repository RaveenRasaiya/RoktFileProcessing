using Rokt.Application.Interfaces;
using Rokt.Domain;
using Rokt.Domain.Requests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Rokt.Application
{
    public class FileService : IFileService
    {
        private readonly IValidationService _validationService;
        private readonly IRecordService _recordService;

        public FileService(IValidationService validationService, IRecordService recordService)
        {
            _validationService = validationService == null ? throw new ArgumentNullException(nameof(validationService)) : validationService;
            _recordService = recordService == null ? throw new ArgumentNullException(nameof(recordService)) : recordService;
        }
        public IEnumerable<LineFeed> ReadFile(EventSearchRequest eventSearchRequest)
        {
            _validationService.Validate(eventSearchRequest);

            if (!File.Exists(eventSearchRequest.FilePath))
            {
                throw new FileNotFoundException($"{eventSearchRequest.FilePath} not found");
            }

            var feeds = new ConcurrentBag<LineFeed>();
            Parallel.ForEach(
                File.ReadLines(eventSearchRequest.FilePath),
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (line, state, index) =>
                {
                    var entity = _recordService.ExtractLineFeed(line, eventSearchRequest.FeedSeparator, eventSearchRequest.StartDate, eventSearchRequest.EndDate);
                    if (entity != null)
                    {
                        feeds.Add(entity);
                    }
                }
            );
            return feeds;
        }
    }
}
