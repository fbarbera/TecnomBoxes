using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using TecnomBoxes.Models;

namespace TecnomBoxes.Services
{
    public interface IWorkshopsService
    {
        Task<IEnumerable<WorkshopDTO>> GetActiveWorkshopsAsync();
        Task<bool> IsActiveWorkshopAsync(int placeId);
    }

    public class WorkshopsService : IWorkshopsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "workshops_active";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        private string _userName = "max@tecnom.com.ar";
        private string _password = "b0x3sApp";
        private string _baseUrl = "https://dev.tecnomcrm.com/api/v1/places/workshops";

        public WorkshopsService(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            var authToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{_userName}:{_password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authToken);
        }
        public async Task<IEnumerable<WorkshopDTO>> GetActiveWorkshopsAsync()
        {
            if (_memoryCache.TryGetValue(CacheKey, out IEnumerable<WorkshopDTO> activeWorkshops))
            {
                return activeWorkshops;
            }
            var response = await _httpClient.GetAsync(_baseUrl);
            response.EnsureSuccessStatusCode();
            var workshopsJson = await response.Content.ReadAsStringAsync();
            var workshops = JsonSerializer.Deserialize<IEnumerable<WorkshopDTO>>(workshopsJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            var activeWorkshopsList = workshops?.Where(w => w.Active) ?? Enumerable.Empty<WorkshopDTO>();
            _memoryCache.Set(CacheKey, activeWorkshopsList, CacheDuration);
            return activeWorkshopsList;

        }
        public async Task<bool> IsActiveWorkshopAsync(int placeId)
        {
            var list= await GetActiveWorkshopsAsync();
            return list.Any(x => x.Id == placeId);
        }
    }
}
