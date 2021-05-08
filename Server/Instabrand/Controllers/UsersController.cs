using Instabrand.Extensions;
using Instabrand.Queries.Users;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public sealed class UsersController : ControllerBase
    {
        /// <summary>
        /// Get user details
        /// </summary>
        [Authorize(Policy = "user")]
        [HttpGet("/users/details")]
        [ProducesResponseType(typeof(UserDetailsView), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult> GetSample(
            CancellationToken cancellationToken,
            [FromServices] IQueryProcessor queryProcessor)
        {
            var user = await queryProcessor.Process(new UserDetailsQuery(User.GetId()), cancellationToken);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
