using Newtonsoft.Json;

namespace Instabrand.Infrastructure.Instagram.ResponseViews
{
    public sealed class DownloadMediaResponseView
    {
        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }
    }
}
