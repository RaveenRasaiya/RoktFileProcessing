using Rokt.Domain;
using Rokt.Domain.Requests;
using System;
using System.Collections.Generic;

namespace Rokt.Application.Interfaces
{
    public interface IFileService
    {
        IEnumerable<LineFeed> ReadFile(EventSearchRequest eventSearchRequest);
    }
}
