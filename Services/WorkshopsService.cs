using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Runtime;
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
        private readonly TecnomCRMSettings _settings;

        public WorkshopsService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClient = httpClientFactory.CreateClient("TecnomCRM");
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<WorkshopDTO>> GetActiveWorkshopsAsync()
        {
            if (_memoryCache.TryGetValue(CacheKey, out IEnumerable<WorkshopDTO> activeWorkshops))
            {
                return activeWorkshops;
            }
            var response = await _httpClient.GetAsync(string.Empty);
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
