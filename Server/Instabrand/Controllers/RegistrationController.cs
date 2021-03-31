﻿using Instabrand.Domain.Registration;
using Instabrand.Extensions;
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

        /// <summary>
        /// Confirm user email
        /// </summary>
        /// <param name="binding">Confirmation model</param>
        /// <response code="204">Successfully</response>
        /// <response code="404">Email not found</response>
        /// <response code="422">Wrong confirmation code</response>
        [HttpPost("/registrations/confirm")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 422)]
        public async Task<IActionResult> Confirm(
            CancellationToken cancellationToken,
            [FromBody] RegistrationConfirmationBinding binding,
            [FromServices] IUserRepository userRepository,
            [FromServices] IConfirmationCodeProvider confirmationCodeProvider)
        {
            if (!confirmationCodeProvider.Validate(binding.ConfirmationCode, out var email))
                return UnprocessableEntity();

            var user = await userRepository.FindByEmail(email, cancellationToken);

            if (user == null)
                return NotFound();

            try
            {
                user.Confirm();
                await userRepository.Save(user);
            }
            catch (UserEmailAlreadyConfirmedException)
            {
                return NoContent();
            }

            return NoContent();
        }

        /// <summary>
        /// Get email confirmation code (temporary method)
        /// </summary>
        /// <response code="204">Successfully</response>
        /// <response code="400">Email must be not empty</response>
        /// <response code="404">Email not found</response>
        /// <response code="422">Email does not require confirmation</response>
        [HttpGet("/registrations/confirmationcode")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> ResendConfirmationCodeForFaceterCustomer(
            CancellationToken cancellationToken,
            [FromQuery] string email,
            [FromServices] IUserRepository userRepository,
            [FromServices] IConfirmationCodeProvider confirmationCodeProvider)
        {
            if (email == null)
                return BadRequest();

            var user = await userRepository.FindByEmail(email, cancellationToken);

            if (user == null)
                return NotFound();

            if (user.EmailState != EmailState.Unconfirmed)
                return UnprocessableEntity();

            var confirmationCode = confirmationCodeProvider.Generate(user.Email);

            return Ok(confirmationCode);
        }
    }
}
