using Instabrand.Domain.Registration;
using Instabrand.Models.Registration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public sealed class RegistrationController : ControllerBase
    {
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="binding">Registration model</param>
        /// <response code="204">Successfully</response>
        /// <response code="400">Invalid registration parameters format</response>
        /// <response code="409">User with that email already registered</response>
        [HttpPost("/registrations")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 409)]
        [ProducesResponseType(typeof(ProblemDetails), 412)]
        public async Task<IActionResult> Registration(
            CancellationToken cancellationToken,
            [FromBody] RegistrationBinding binding,
            [FromServices] IUserRepository userRepository,
            [FromServices] UserRegistrationService registrationService)
        {
            var user = await userRepository.FindByEmail(binding.Email, cancellationToken);

            if (user != null)
            {
                if (user.EmailState == EmailState.Unconfirmed)
                    return NoContent();
                return Conflict();
            }

            user = registrationService.CreateUser(binding.Email, binding.Password);

            try
            {
                await userRepository.Save(user);

                return NoContent();
            }
            catch (DbUpdateException exception)
                when (((Npgsql.PostgresException)exception.InnerException)?.SqlState == "23505") // 23505 unique_violation
            {
                return Conflict();
            }
        }
    }
}
