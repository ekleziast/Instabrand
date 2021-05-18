using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Instabrand.Models.InstapagesConstructor
{
    public sealed class UploadImagesBinding
    {
        [FromForm]
        public IFormFile Favicon { get; set; }

        [FromForm]
        public IFormFile Background { get; set; }
    }

    public class UploadImagesBindingValidator : AbstractValidator<UploadImagesBinding>
    {
        public UploadImagesBindingValidator()
        {
            RuleFor(o => o.Favicon)
                .NotNull();
            RuleFor(o => o.Background)
                .NotNull();
        }
    }
}
