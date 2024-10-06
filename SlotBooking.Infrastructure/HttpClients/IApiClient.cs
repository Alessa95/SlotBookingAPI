
namespace SlotBooking.Infrastructure.HttpClients
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(string endpoint);
        Task PostAsync(string endpoint, object data);
        Task<T> PostAsync<T>(string endpoint, object data);
    }
}