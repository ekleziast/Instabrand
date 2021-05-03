using Instabrand.Infrastructure.Instagram.ResponseViews;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Infrastructure.Instagram
{
    public sealed class InstagramApi
    {
        private readonly string _appId;
        private readonly string _redirectUri;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public InstagramApi(IOptions<InstagramApiOptions> options, HttpClient httpClient)
        {
            _appId = options.Value.AppId;
            _redirectUri = options.Value.RedirectUri;
            _secretKey = options.Value.SecretKey;
            _httpClient = httpClient;
        }

        public async Task<AuthResponseView> Auth(string code, CancellationToken cancellationToken)
        {
            var requestContent = new List<KeyValuePair<string, string>>();
            requestContent.Add(new KeyValuePair<string, string>("client_id", _appId));
            requestContent.Add(new KeyValuePair<string, string>("client_secret", _secretKey));
            requestContent.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            requestContent.Add(new KeyValuePair<string, string>("code", code));
            requestContent.Add(new KeyValuePair<string, string>("redirect_uri", _redirectUri));

            var response = await _httpClient.PostAsync(@"oauth/access_token", new FormUrlEncodedContent(requestContent), cancellationToken);

            if (!response.IsSuccessStatusCode)
                throw new ApiException
                {
                    StatusCode = response.StatusCode,
                    Reason = response.ReasonPhrase
                };

            var responseView = JsonConvert.DeserializeObject<AuthResponseView>(await response.Content.ReadAsStringAsync());
            return responseView;
        }
    }
}
