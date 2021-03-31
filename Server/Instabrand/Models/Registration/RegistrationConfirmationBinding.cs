using FluentValidation;

namespace Instabrand.Models.Registration
{
    public sealed class RegistrationConfirmationBinding
    {
        /// <summary>
        /// User confirmation code from email
        /// </summary>
        public string ConfirmationCode { get; set; }
    }

    public sealed class RegistrationConfirmationBindingValidator : AbstractValidator<RegistrationConfirmationBinding>
    {
        public RegistrationConfirmationBindingValidator()
        {
            RuleFor(b => b.ConfirmationCode)
                .NotEmpty();
        }
    }
}
