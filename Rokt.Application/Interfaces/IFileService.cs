using Rokt.Domain;
using Rokt.Domain.Requests;
using System.Collections.Generic;

namespace Rokt.Application.Interfaces
{
    public interface IFileService
    {
        IEnumerable<LineFeed> Process(EventSearchRequest eventSearchRequest);
    }
}