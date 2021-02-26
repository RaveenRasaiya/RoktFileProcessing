using Rokt.Domain.Requests;

namespace Rokt.Application.Interfaces
{
    public interface IDataService
    {
        string ExtractData(EventSearchRequest eventSearchRequest);
    }
}
