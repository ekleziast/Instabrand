using System;
using System.Text.Json.Serialization;

namespace Instabrand.Models.Authentication
{
    public sealed class TokenView
    {
        /// <summary>
        /// The access token issued by the authorization server
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; }

        /// <summary>
        /// The type of the token issued. Example: Bearer 
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; }

        /// <summary>
        /// The lifetime in seconds of the access token
        /// </summary>
        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; }

        /// <summary>
        /// The refresh token, which can be used to obtain new  access tokens using the same authorization grant
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; }

        public TokenView(string accessToken, TimeSpan expiresIn, string refreshToken)
        {
            AccessToken = accessToken;
            TokenType = "Bearer";
            ExpiresIn = (long)expiresIn.TotalSeconds;
            RefreshToken = refreshToken;
        }
    }
}
