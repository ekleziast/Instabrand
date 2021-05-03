using Newtonsoft.Json;

namespace Instabrand.Infrastructure.Instagram.ResponseViews
{
    public sealed class AuthResponseView
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
