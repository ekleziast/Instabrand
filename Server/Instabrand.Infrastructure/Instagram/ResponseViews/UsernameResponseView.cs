using Newtonsoft.Json;

namespace Instabrand.Infrastructure.Instagram.ResponseViews
{
    public sealed class UsernameResponseView
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
