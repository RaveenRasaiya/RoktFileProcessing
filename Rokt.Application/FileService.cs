using Rokt.Application.Interfaces;
using Rokt.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rokt.Application
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Read line feed from the file as much as fast, then apply order if there
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<LineFeed> ReadFile(string filePath, DateTime startDate, DateTime endDate)
        {
            var result = new ConcurrentBag<LineFeed>();
            Parallel.ForEach(
                File.ReadLines(filePath),
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                (line, state, index) =>
                {
                    var entity = ExtractLineFeed(line, startDate, endDate);
                    if (entity != null)
                    {
                        result.Add(entity);
                    }
                }
            );

            return result.OrderBy(l => l.EventTime);
        }

        private LineFeed ExtractLineFeed(string lineData, DateTime startDate, DateTime dateTime)
        {
            string[] feedArray = lineData.Split(' ');
            if (DateTime.TryParse(feedArray[0], out DateTime _lineDate))
            {
                if (_lineDate >= startDate && _lineDate <= dateTime)
                {
                    return new LineFeed
                    {
                        EventTime = _lineDate,
                        Email = feedArray[1],
                        SessionId = feedArray[2]
                    };
                }
            }
            return default;
        }
    }
}
