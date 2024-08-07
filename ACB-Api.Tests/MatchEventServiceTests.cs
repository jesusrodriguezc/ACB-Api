using ACB_Api.Models;
using ACB_Api.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace ACB_Api.Tests {
	public class MockHttpMessageHandler : HttpMessageHandler {
		private readonly HttpResponseMessage _response;

		public MockHttpMessageHandler (HttpResponseMessage response) {
			_response = response;
		}

		protected override async Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken) {
			return await Task.FromResult(_response);
		}
	}

	[TestFixture]
	public class MatchEventServiceTests {
		private Mock<IMemoryCache> _mockMemoryCache;
		private Mock<IOptions<ApiSettings>> _mockApiSettings;
		private HttpClient _httpClient;
		private MatchEventService _service;

		[SetUp]
		public void Setup () {
			_mockMemoryCache = new Mock<IMemoryCache>();
			_mockApiSettings = new Mock<IOptions<ApiSettings>>();
			var apiSettings = new ApiSettings { 
				BaseUrl = "https://api2.acb.com/api/v1/openapilive/PlayByPlay/matchevents?idMatch={0}", 
				Token = "H4sIAAAAAAAAA32Ry3aqMBSG36iLi7qOw4oFk9PEQyohZEYilkCirIMF5OkbOmiVQUdZ+/bv798pbrAUkVR7BUEyAhcr0IIzWcoArEDdMBrA9VNxg5BPiQqp1wDGnEGTp/009MIZD4sd0dIsSxFMw7ATUaL2ulWxN7jZGV2poTegehuvrzKiTp7iRniL7zxhoZuzTZl5uhPKTTkD055Q+qTkkR6Pka4y9qNDTDjyu/hg6zxdVnkajq+UazmZ0Bstz7gT5j5HGhmtx4MPa9s/34dyxvUj41LL22OdMOxkqdXxSZN5d9r10BwNDS2v5Vh/zPg8ntD6W9OhwzHVvfDnPU033ZFHL495uinlmTTcsv/q5Yshmc+2GdNB8fXWs32wK3a4Ej7UMx8JT13LBy3LvUdsNXDDjf2P2X3YW6+EgQZUlwEf6gXeIgdvk4W9my52z2pfAReNcY8q1KPte4+CXtn/cub9p/jJef7j/Xvf/j150Sag6C1ctS41qVe3p6Fml5U4gf/xdecli8sn/TA0Eb8CAAA=" };
			_mockApiSettings.Setup(s => s.Value).Returns(apiSettings);

			// Setup HttpClient with a mock HttpMessageHandler
			var response = new HttpResponseMessage {
				StatusCode = HttpStatusCode.OK,
				Content = new StringContent(JsonConvert.SerializeObject(new List<MatchEvent> {
					new MatchEvent { IdTeam = 1, IdLicense = 1, Crono = "00:10", IdPlaybyplaytype = 1 },
					new MatchEvent { IdTeam = 2, IdLicense = 2, Crono = "00:20", IdPlaybyplaytype = 2 }
				}))
			};

			var handler = new MockHttpMessageHandler(response);
			_httpClient = new HttpClient(handler);

			_service = new MatchEventService(_httpClient, _mockMemoryCache.Object, _mockApiSettings.Object);
		}

		[Test]
		public async Task GetMatchEventsAsync_ReturnsDataFromApi () {
			// Arrange
			var gameId = 1L;
			var cacheKey = $"pbp_{gameId}";
			object cacheValue;
			_mockMemoryCache.Setup(mc => mc.TryGetValue(cacheKey, out cacheValue)).Returns(false);
			// Setup the cache entry
			var mockCacheEntry = new Mock<ICacheEntry>();
			_mockMemoryCache.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
							.Returns(mockCacheEntry.Object);
			// Act
			var result = await _service.GetMatchEventsAsync(gameId);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.InstanceOf<IEnumerable<MatchEvent>>());
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public async Task GetMatchEventsAsync_ReturnsDataFromCache () {
			// Arrange
			var gameId = 1L;
			var cacheKey = $"pbp_{gameId}";
			var cachedData = new List<MatchEvent> {
				new MatchEvent { IdTeam = 1, IdLicense = 1, Crono = "00:10", IdPlaybyplaytype = 1 },
				new MatchEvent { IdTeam = 2, IdLicense = 2, Crono = "00:20", IdPlaybyplaytype = 2 }
			};
			object cacheValue = cachedData;
			_mockMemoryCache.Setup(mc => mc.TryGetValue(cacheKey, out cacheValue)).Returns(true);

			// Act
			var result = await _service.GetMatchEventsAsync(gameId);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result, Is.InstanceOf<IEnumerable<MatchEvent>>());
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public void GetMatchEventsAsync_ThrowsException_OnHttpError () {
			// Arrange
			var gameId = 1L;
			var response = new HttpResponseMessage {
				StatusCode = HttpStatusCode.InternalServerError
			};

			var handler = new MockHttpMessageHandler(response);
			_httpClient = new HttpClient(handler);
			_service = new MatchEventService(_httpClient, _mockMemoryCache.Object, _mockApiSettings.Object);

			// Act & Assert
			Assert.ThrowsAsync<HttpRequestException>(async () => await _service.GetMatchEventsAsync(gameId));
		}

		[Test]
		public async Task GetMatchEventsAsync_CachesData () {
			// Arrange
			var gameId = 1L;
			var cacheKey = $"pbp_{gameId}";
			object cacheValue;
			_mockMemoryCache.Setup(mc => mc.TryGetValue(cacheKey, out cacheValue)).Returns(false);

			List<MatchEvent> playByPlayList = new List<MatchEvent> {
				new MatchEvent { IdTeam = 1, IdLicense = 1, Crono = "00:10", IdPlaybyplaytype = 1 },
				new MatchEvent { IdTeam = 2, IdLicense = 2, Crono = "00:20", IdPlaybyplaytype = 2 }
			};

			// Setup the cache entry
			var mockCacheEntry = new Mock<ICacheEntry>();
			_mockMemoryCache.Setup(mc => mc.CreateEntry(It.IsAny<object>()))
							.Returns(mockCacheEntry.Object);

			// Act
			var result = await _service.GetMatchEventsAsync(gameId);

			// Assert
			_mockMemoryCache.Verify(mc => mc.CreateEntry(cacheKey), Times.Once);
			mockCacheEntry.VerifySet(entry => entry.Value = It.IsAny<object>(), Times.Once);
		}
	}
}
