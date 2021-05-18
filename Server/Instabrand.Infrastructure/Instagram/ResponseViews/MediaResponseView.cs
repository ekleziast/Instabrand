using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Instabrand.Infrastructure.Instagram.ResponseViews
{
    public sealed class MediaResponseView
    {
        [JsonProperty("data")]
        public IEnumerable<DataView> Data { get; set; }
    }

    public sealed class DataView
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("caption")]
        public string Description { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
