using Instabrand.Extensions;
using Instabrand.Infrastructure.Instagram.ResponseViews;
using Instabrand.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public sealed class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// User authentication
        /// </summary>
        /// <param name="binding">Authentication model</param>
        /// <response code="200">Successfully</response>
        /// <response code="400">Bad request</response>
        [HttpPost("oauth2/token")]
        [ProducesResponseType(typeof(TokenView), 200)]
        [ProducesResponseType(typeof(ErrorView), 400)]
        public async Task<IActionResult> Authentication(
            CancellationToken cancellationToken,
            [FromForm] AuthenticationBinding binding,
            [FromServices] Domain.Authentication.UserAuthenticationService authenticationService)
        {
            switch (binding.GrantType)
            {
                case GrantType.Password:
                    try
                    {
                        var token = await authenticationService.AuthenticationByPassword(binding.UserName, binding.Password, cancellationToken);
                        return Ok(new TokenView(token.AccessToken, token.ExpiresIn, token.RefreshToken));
                    }
                    catch (Domain.Authentication.UnauthorizedException)
                    {
                        return BadRequest(new ErrorView(ErrorCode.UnauthorizedClient, "Email or password is incorrect"));
                    }
                    catch (Domain.Authentication.UnconfirmedException)
                    {
                        return BadRequest(new ErrorView(ErrorCode.InvalidClient, "Email is unconfirmed"));
                    }
                case GrantType.RefreshToken:
                    try
                    {
                        var token = await authenticationService.AuthenticationByRefreshToken(binding.RefreshToken, cancellationToken);
                        return Ok(new TokenView(token.AccessToken, token.ExpiresIn, token.RefreshToken));
                    }
                    catch (Domain.Authentication.UnauthorizedException)
                    {
                        return BadRequest(new ErrorView(ErrorCode.UnauthorizedClient, "Refresh token is incorrect"));
                    }
                default:
                    return BadRequest(new ErrorView(ErrorCode.UnsupportedGrantType, $"Unsupported grant type: {binding.GrantType}"));
            }
        }

        /// <summary>
        /// Facebook authentication
        /// </summary>
        /// <param name="fbBinding">Authentication model</param>
        /// <response code="201">Successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="409">Instagram already registered</response>
        [Authorize(Policy = "user")]
        [HttpPost("oauth2/fb")]
        [ProducesResponseType(201)]
        [ProducesResponseType(409)]
        [ProducesResponseType(typeof(ErrorView), 400)]
        public async Task<IActionResult> FbAuthentication(
            CancellationToken cancellationToken,
            [FromForm] FbAuthenticationBinding fbBinding,
            [FromServices] Infrastructure.Instagram.InstapageCreationService creationService,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository)
        {
            try
            {
                var instapage = await creationService.CreateInstapage(User.GetId(), fbBinding.Code, cancellationToken);

                var instapageExists = await instapageRepository.GetByInstagram(instapage.InstagramLogin, cancellationToken);

                if (instapageExists != null)
                    return Conflict();

                await instapageRepository.Save(instapage);

                return NoContent();
            }
            catch (Infrastructure.Instagram.ApiException ex)
            {
                return BadRequest(new ErrorView(ErrorCode.InstagramException, ex.Reason));
            }
        }
    }
}
