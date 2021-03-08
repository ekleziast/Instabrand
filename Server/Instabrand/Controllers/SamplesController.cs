using Instabrand.Queries.Samples;
using Instabrand.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Instabrand.Controllers
{
    [ApiController]
    public class SamplesController : ControllerBase
    {
        /// <summary>
        /// Get sample details
        /// </summary>
        /// <param name="id">Sample identifier</param>
        [HttpGet("/samples/{id}")]
        [ProducesResponseType(typeof(SampleView), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult> GetSample(
            CancellationToken cancellationToken,
            [FromRoute] Guid id,
            [FromServices] IQueryProcessor queryProcessor)
        {
            var sample = await queryProcessor.Process(new SampleQuery(id), cancellationToken);
            if (sample == null)
                return NotFound();

            return Ok(sample);
        }

        /// <summary>
        /// Get samples list
        /// </summary>
        [HttpGet("/owner/{ownerId}/samples")] // it should be another controller -> OwnersController
        [ProducesResponseType(typeof(SampleView), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        public async Task<ActionResult> GetSample(
            CancellationToken cancellationToken,
            [FromServices] IQueryProcessor queryProcessor,
            [FromRoute] Guid ownerId,
            [FromQuery] int offset = 0,
            [FromQuery] int limit = 10)
        {
            return Ok(await queryProcessor.Process(new SampleListQuery(ownerId, offset, limit), cancellationToken));
        }
    }
}
