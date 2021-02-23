using Rokt.Domain;
using System;
using System.Collections.Generic;

namespace Rokt.Application.Interfaces
{
    public interface IDataService
    {
        IEnumerable<LineFeed> ExtraData(string filePath, DateTime startDate, DateTime endDate);
    }
}
