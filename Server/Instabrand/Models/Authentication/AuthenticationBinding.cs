using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Instabrand.Models.Authentication
{
    public sealed class AuthenticationBinding
    {
        /// <summary>
        /// Grant type
        /// </summary>
        [FromForm(Name = "grant_type")]
        public GrantType GrantType { get; set; }

        /// <summary>
        /// Email (grant_type == password)
        /// </summary>
        [FromForm(Name = "username")]
        public string UserName { get; set; }

        /// <summary>
        /// Password (grant_type == password)
        /// </summary>
        [FromForm(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Refresh token (grant_type == refreshtoken)
        /// </summary>
        [FromForm(Name = "refresh_token")]
        public string RefreshToken { get; set; }
    }

    public class AuthenticationBindingValidator : AbstractValidator<AuthenticationBinding>
    {
        public AuthenticationBindingValidator()
        {
            RuleFor(o => o.GrantType)
                .NotNull();

            RuleFor(o => o.UserName)
                .NotEmpty()
                .When(o => o.GrantType == GrantType.Password);
            RuleFor(o => o.Password)
                .NotEmpty()
                .When(o => o.GrantType == GrantType.Password);

            RuleFor(o => o.RefreshToken)
                .NotEmpty()
                .When(o => o.GrantType == GrantType.RefreshToken);
        }
    }

    public enum GrantType
    {
        Password,
        RefreshToken
    }
}
