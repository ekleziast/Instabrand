using System.Runtime.Serialization;

namespace Instabrand.Models.Authentication
{
    public enum ErrorCode
    {
        [EnumMember(Value = "invalid_request")]
        InvalidRequest,
        [EnumMember(Value = "invalid_client")]
        InvalidClient,
        [EnumMember(Value = "invalid_grant")]
        InvalidGrant,
        [EnumMember(Value = "unauthorized_client")]
        UnauthorizedClient,
        [EnumMember(Value = "unsupported_grant_type")]
        UnsupportedGrantType,
        [EnumMember(Value = "invalid_scope")]
        InvalidScope,
        [EnumMember(Value = "instagram_exception")]
        InstagramException
    }
}
