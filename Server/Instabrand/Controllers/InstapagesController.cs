using Instabrand.Queries.Instapages;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public sealed class InstapagesController : ControllerBase
    {
        /// <summary>
        /// Get instapage background
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <response code="200">Successfully</response>
        /// <response code="400">Instapage is not enabled</response>
        /// <response code="404">Instapage or background for this instapage not found</response>
        /// <response code="422">Background has unprocessable extension</response>
        [HttpGet("/instapages/{login}/background")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetBackground(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository,
            [FromServices] Domain.IFileStorage fileStorage)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);
            if (instapage == null)
                return NotFound();

            // Feature Work In Progress [WIP]
            //if (instapage.State != Domain.Instapage.InstapageState.Enabled)
            //    return BadRequest();

            try
            {
                (var fs, var extension) = fileStorage.GetFileWithoutExtension("background", login);

                switch (extension)
                {
                    case ".png":
                        return File(fs, "image/png");
                    case ".jpg":
                    case ".jpeg":
                        return File(fs, "image/jpeg");
                    default:
                        return UnprocessableEntity();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get instapage favicon
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <response code="200">Successfully</response>
        /// <response code="400">Instapage is not enabled</response>
        /// <response code="404">Instapage or favicon for this instapage not found</response>
        /// <response code="422">Favicon has unprocessable extension</response>
        [HttpGet("/instapages/{login}/favicon")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetFavicon(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository,
            [FromServices] Domain.IFileStorage fileStorage)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);
            if (instapage == null)
                return NotFound();

            // Feature Work In Progress [WIP]
            //if (instapage.State != Domain.Instapage.InstapageState.Enabled)
            //    return BadRequest();

            try
            {
                (var fs, var extension) = fileStorage.GetFileWithoutExtension("favicon", login);

                switch (extension)
                {
                    case ".png":
                        return File(fs, "image/png");
                    case ".jpg":
                    case ".jpeg":
                        return File(fs, "image/jpeg");
                    case ".ico":
                        return File(fs, "image/x-icon");
                    default:
                        return UnprocessableEntity();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get instapage image
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <param name="id">Instapost id</param>
        /// <response code="200">Successfully</response>
        /// <response code="400">Instapage is not enabled</response>
        /// <response code="404">Instapage or image for this instapage not found</response>
        /// <response code="422">Image has unprocessable extension</response>
        [HttpGet("/instapages/{login}/images")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> GetImage(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromQuery] string id,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository,
            [FromServices] Domain.IFileStorage fileStorage)
        {
            var instapage = await instapageRepository.GetByInstagram(login, cancellationToken);
            if (instapage == null)
                return NotFound();

            // Feature Work In Progress [WIP]
            //if (instapage.State != Domain.Instapage.InstapageState.Enabled)
            //    return BadRequest();

            try
            {
                (var fs, var extension) = fileStorage.GetFileWithoutExtension(id, login);

                switch (extension)
                {
                    case ".png":
                        return File(fs, "image/png");
                    case ".jpg":
                    case ".jpeg":
                        return File(fs, "image/jpeg");
                    case ".ico":
                        return File(fs, "image/x-icon");
                    default:
                        return UnprocessableEntity();
                }
            }
            catch
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get Instapage
        /// </summary>
        /// <param name="login">Instagram login</param>
        /// <response code="200">Successful</response>
        /// <response code="400">Instapage is not enabled</response>
        /// <response code="400">Instapage not found</response>
        [HttpGet("/instapages/{login}")]
        [ProducesResponseType(typeof(InstapageView), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetInstapage(
            CancellationToken cancellationToken,
            [FromRoute] string login,
            [FromServices] Domain.Instapage.IInstapageRepository instapageRepository,
            [FromServices] IQueryProcessor queryProcessor)
        {
            var instapage = await queryProcessor.Process(new InstapageQuery(login), cancellationToken);
            if (instapage == null)
                return NotFound();

            // Feature Work In Progress [WIP]
            //if (instapage.State != InstapageState.Enabled)
            //    return BadRequest();

            return Ok(instapage);
        }
    }
}
