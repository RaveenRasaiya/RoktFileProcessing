namespace Rokt.Application.Interfaces
{
    public interface IValidationService
    {
        void Validate<T>(T request);
    }
}