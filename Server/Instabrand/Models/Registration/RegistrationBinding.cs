using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Instabrand.Models.Registration
{
    public sealed class RegistrationBinding
    {
        /// <summary>
        /// User email
        /// </summary>
        [FromForm(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [FromForm(Name = "password")]
        public string Password { get; set; }
    }

    public sealed class RegistrationBindingValidator : AbstractValidator<RegistrationBinding>
    {
        public RegistrationBindingValidator()
        {
            RuleFor(b => b.Email)
                .NotNull()
                .EmailAddress();
            RuleFor(b => b.Password)
                .NotNull()
                .MinimumLength(6);
        }
    }
}
