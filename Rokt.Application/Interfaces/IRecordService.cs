using Rokt.Domain;
using System;

namespace Rokt.Application.Interfaces
{
    public interface IRecordService
    {
        LineFeed ExtractLineFeed(string line, char separator, DateTime startDate, DateTime endDate);
    }
}