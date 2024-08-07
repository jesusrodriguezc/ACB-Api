using ACB_Api.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ACB_Api.Services {
	public class MatchEventService : IMatchEventService{
		private readonly HttpClient _httpClient;
		private readonly IMemoryCache _cache;
		private readonly ApiSettings _apiSettings;

		public MatchEventService (HttpClient httpClient, IMemoryCache cache, IOptions<ApiSettings> apiSettings) {
			_httpClient = httpClient;
			_cache = cache;
			_apiSettings = apiSettings.Value;
		}

		public async Task<IEnumerable<MatchEvent>> GetMatchEventsAsync (long gameId) {
			string cacheKey = $"pbp_{gameId}";

			if (_cache.TryGetValue(cacheKey, out List<MatchEvent> cachedData)) {
				return cachedData;
			}

			string url = string.Format(_apiSettings.BaseUrl, gameId);

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiSettings.Token);

			HttpResponseMessage response = await _httpClient.GetAsync(url);
			response.EnsureSuccessStatusCode();

			string responseBody = await response.Content.ReadAsStringAsync();
			var playByPlayList = JsonConvert.DeserializeObject<List<MatchEvent>>(responseBody);

			_cache?.Set(cacheKey, playByPlayList, TimeSpan.FromMinutes(30));

			return playByPlayList;
		}
	}
}
