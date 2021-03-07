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
            [FromRoute] Guid id,
            [FromServices] IQueryProcessor queryProcessor,
            CancellationToken cancellationToken)
        {
            var sample = await queryProcessor.Process(new SampleQuery(id), cancellationToken);
            if (sample == null)
                return NotFound();

            return Ok(sample);
        }
    }
}
