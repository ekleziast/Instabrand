using System.Text.Json.Serialization;

namespace Instabrand.Models.Authentication
{
    public sealed class ErrorView
    {
        /// <summary>
        /// Error code https://tools.ietf.org/html/rfc6749#section-5.2
        /// </summary>
        [JsonPropertyName("error")]
        public ErrorCode Error { get; }

        /// <summary>
        /// Human-readable text providing additional information
        /// </summary>
        [JsonPropertyName("error_description")]
        public string ErrorDescription { get; }

        public ErrorView(ErrorCode error, string errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }
    }
}
