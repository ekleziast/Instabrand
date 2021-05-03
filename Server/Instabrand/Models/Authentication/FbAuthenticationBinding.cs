using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Instabrand.Models.Authentication
{
    public sealed class FbAuthenticationBinding
    {

        /// <summary>
        /// Temporary authentication code
        /// </summary>
        [FromForm(Name = "code")]
        public string Code { get; set; }
    }

    public class FbAuthenticationBindingValidator : AbstractValidator<FbAuthenticationBinding>
    {
        public FbAuthenticationBindingValidator()
        {
            RuleFor(o => o.Code)
                .NotEmpty();
        }
    }
}
