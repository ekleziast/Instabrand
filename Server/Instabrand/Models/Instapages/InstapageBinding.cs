using FluentValidation;
using System.Collections.Generic;

namespace Instabrand.Models.Instapages
{
    public sealed class InstapageBinding
    {
        /// <summary>
        /// Instapage title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Instapage description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Instapage posts
        /// </summary>
        public IEnumerable<PostBinding> Posts { get; set; }
    }

    public sealed class PostBinding
    {
        /// <summary>
        /// Instapost Instagram id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Instapost title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Instapost description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Instapost currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Instapost price
        /// </summary>
        public decimal Price { get; set; }
    }

    public class InstapageBindingValidator : AbstractValidator<InstapageBinding>
    {
        public InstapageBindingValidator()
        {
            RuleFor(o => o.Title)
                .NotEmpty();
            RuleFor(o => o.Description)
                .NotEmpty();

            RuleForEach(o => o.Posts)
                .ChildRules(post =>
                {
                    post.RuleFor(s => s.Id)
                        .NotEmpty();
                    post.RuleFor(s => s.Title)
                        .MaximumLength(128)
                        .NotEmpty();
                    post.RuleFor(s => s.Currency)
                        .MaximumLength(128)
                        .NotEmpty();
                    post.RuleFor(s => s.Price)
                        .NotNull();
                });
        }
    }
}
