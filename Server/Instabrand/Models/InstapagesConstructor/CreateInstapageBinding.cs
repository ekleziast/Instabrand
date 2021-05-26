using FluentValidation;
using System;
using System.Collections.Generic;

namespace Instabrand.Models.InstapagesConstructor
{
    public sealed class CreateInstapageBinding
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

        /// <summary>
        /// VK social link
        /// </summary>
        public string Vkontakte { get; set; }

        /// <summary>
        /// Telegram social link
        /// </summary>
        public string Telegram { get; set; }
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

        /// <summary>
        /// Instapost timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    public class InstapageBindingValidator : AbstractValidator<CreateInstapageBinding>
    {
        public InstapageBindingValidator()
        {
            RuleFor(o => o.Title)
                .NotEmpty();
            RuleFor(o => o.Description)
                .NotEmpty();

            RuleFor(o => o.Vkontakte)
                .MaximumLength(1024);
            RuleFor(o => o.Telegram)
                .MaximumLength(1024);

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
                    post.RuleFor(s => s.Timestamp)
                        .NotEmpty();
                });
        }
    }
}
