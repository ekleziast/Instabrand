using Instabrand.Infrastructure.Instagram.ResponseViews;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Instagram
{
    public sealed class InstagramGraphApi
    {
        private readonly HttpClient _httpClient;

        public InstagramGraphApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UsernameResponseView> GetUsername(string accessToken, CancellationToken cancellationToken)
        {
            var query = new Dictionary<string, string>
            {
                ["fields"] = "username",
                ["access_token"] = accessToken,
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("me", query), cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new ApiException
                {
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase
                };

            var responseView = JsonConvert.DeserializeObject<UsernameResponseView>(await response.Content.ReadAsStringAsync());
            return responseView;
        }

        public async Task<MediaResponseView> GetMedia(string accessToken, CancellationToken cancellationToken)
        {
            var query = new Dictionary<string, string>
            {
                ["access_token"] = accessToken,
                ["fields"] = "id,caption,media_type,media_url,permalink,timestamp"
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString("me/media", query), cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new ApiException
                {
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase
                };

            var responseView = JsonConvert.DeserializeObject<MediaResponseView>(await response.Content.ReadAsStringAsync());
            return responseView;
        }

        public async Task<Stream> DownloadMedia(string accessToken, string mediaId, CancellationToken cancellationToken)
        {
            var query = new Dictionary<string, string>
            {
                ["access_token"] = accessToken,
                ["fields"] = "media_url"
            };

            var response = await _httpClient.GetAsync(QueryHelpers.AddQueryString($"{mediaId}", query), cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new ApiException
                {
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase
                };

            var imageUrl = JsonConvert.DeserializeObject<DownloadMediaResponseView>(await response.Content.ReadAsStringAsync());

            var imageResponse = await _httpClient.GetAsync(imageUrl.MediaUrl, cancellationToken);

            if (!imageResponse.IsSuccessStatusCode)
                throw new ApiException
                {
                    StatusCode = imageResponse.StatusCode,
                    Reason = imageResponse.ReasonPhrase
                };

            var imageStream = await imageResponse.Content.ReadAsStreamAsync();

            return imageStream;
        }
    }
}
