using Instabrand.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public sealed class InstagramController : ControllerBase
    {
        [HttpGet("/instagram/{username}")]
        public async Task<ActionResult<InstagramUser>> GetInstagramUser(
            CancellationToken cancellationToken,
            [FromRoute] string username,
            [FromServices] IInstragramService instragram)
        {
            try
            {
                var user = await instragram.GetUser(username, cancellationToken);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (IInstragramService.ServiceUnavailableException)
            {
                return new StatusCodeResult(500);
            }
        }
    }
}
