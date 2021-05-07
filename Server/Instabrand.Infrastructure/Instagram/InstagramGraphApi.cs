using Instabrand.Infrastructure.Instagram.ResponseViews;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Collections.Generic;
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
    }
}
