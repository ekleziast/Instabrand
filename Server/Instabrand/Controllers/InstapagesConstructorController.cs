using Instabrand.Extensions;
using Instabrand.Infrastructure.Instagram.ResponseViews;
using Instabrand.Models.Authentication;
using Instabrand.Models.Instapages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public sealed class InstapagesConstructorController : ControllerBase
    {
        /// <summary>
        /// Get instagram media for constructor
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <response code="200">Successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Instapage for this user not found</response>
        [Authorize(Policy = "user")]
        [HttpGet("/instapages/constructor/{login}/media/")]
        [ProducesResponseType(typeof(IEnumerable<DataView>), 200)]
        [ProducesResponseType(typeof(ErrorView), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetInstagramMedia(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository,
            [FromServices] Infrastructure.Instagram.InstapageCreationService creationService,
            [FromQuery] int limit = 16)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);

            if(instapage == null)
                return NotFound();

            if (instapage.UserId != User.GetId())
                return NotFound();

            try
            {
                var media = await creationService.GetMedia(instapage.AccessToken, limit, cancellationToken);

                return Ok(media);
            }
            catch (Infrastructure.Instagram.ApiException ex)
            {
                return BadRequest(new ErrorView(ErrorCode.InstagramException, ex.Reason));
            }
        }

        /// <summary>
        /// Create Instapage via constructor
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <param name="binding">Binding model</param>
        /// <response code="201">Successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Instapage for this user not found</response>
        [Authorize(Policy = "user")]
        [HttpPost("/instapages/constructor/{login}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorView), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateInstapage(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromBody] InstapageBinding binding,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository,
            [FromServices] Infrastructure.Instagram.InstapageCreationService creationService)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);

            if (instapage == null)
                return NotFound();

            if (instapage.UserId != User.GetId())
                return NotFound();

            instapage.SetInfo(binding.Title, binding.Description);

            try
            {
                foreach(var instapost in binding.Posts)
                {
                    if (instapage.Instaposts.Any(o => o.Id == instapost.Id))
                        continue;

                    await creationService.SaveMedia(instapage.AccessToken, instapage.InstagramLogin, instapost.Id, cancellationToken);
                    instapage.AddPost(new Domain.Instapage.Instapost(
                        id: instapost.Id,
                        instapageId: instapage.Id,
                        title: instapost.Title,
                        description: instapost.Description,
                        price: instapost.Price,
                        currency: instapost.Currency));
                }

                await instapageRepository.Save(instapage);

                return NoContent();
            }
            catch (Infrastructure.Instagram.ApiException ex)
            {
                return BadRequest(new ErrorView(ErrorCode.InstagramException, ex.Reason));
            }
        }

        /// <summary>
        /// Enable Instapage
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <response code="201">Successfully</response>
        /// <response code="404">Instapage for this user not found</response>
        /// <response code="404">Invalid state</response>
        [HttpPost("/instapages/constructor/{login}/enable")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> EnableInstapage(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);

            if (instapage == null)
                return NotFound();

            if (instapage.UserId != User.GetId())
                return NotFound();
            try
            {
                instapage.Enable();
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }

            await instapageRepository.Save(instapage);

            return NoContent();
        }

        /// <summary>
        /// Disable Instapage
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <response code="201">Successfully</response>
        /// <response code="404">Instapage for this user not found</response>
        /// <response code="404">Invalid state</response>
        [HttpPost("/instapages/constructor/{login}/disable")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> DisableInstapage(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);

            if (instapage == null)
                return NotFound();

            if (instapage.UserId != User.GetId())
                return NotFound();
            try
            {
                instapage.Disable();
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(ex.Message);
            }

            await instapageRepository.Save(instapage);

            return NoContent();
        }
    }
}
