using Rokt.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rokt.Application.Interfaces
{
    public interface IFileService
    {
        IEnumerable<LineFeed> ReadFile(string filePath, DateTime startDate, DateTime endDate);
    }
}
