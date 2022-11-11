using System;
using System.Net.Http;
using System.Threading.Tasks;

using IgniteSpotlight.MapsApi.Configs;

using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;

namespace IgniteSpotlight.MapsApi.Services
{
    /// <summary>
    /// This represents the service entity for Naver Map.
    /// </summary>
    public class MockMapService : IMapService
    {
        public const string Name = "Mock";

        private readonly MapsSettings _settings;
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="NaverMapService"/> class.
        /// </summary>
        /// <param name="settings"><see cref="MapsSettings"/> instance.</param>
        /// <param name="factory"><see cref="IHttpClientFactory"/> instance.</param>
        public MockMapService(MapsSettings settings, IHttpClientFactory factory)
        {
            this._settings = settings.ThrowIfNullOrDefault();
            this._http = factory.ThrowIfNullOrDefault().CreateClient("naver");
        }

        /// <inheritdoc/>
        public async Task<byte[]> GetMapAsync(HttpRequest req)
        {
            this._http.DefaultRequestHeaders.Clear();
            var requestUri = new Uri("https://raw.githubusercontent.com/justinyoo/ignite-spotlight-demo/main/images/map.png");

            var bytes = await this._http.GetByteArrayAsync(requestUri).ConfigureAwait(false);

            return bytes;
        }
    }
}