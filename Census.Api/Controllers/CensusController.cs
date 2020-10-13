using Census.Api.Models;
using Census.Core;
using Fusi.Tools.Data;
using Microsoft.AspNetCore.Mvc;

namespace Census.Api.Controllers
{
    /// <summary>
    /// Census data.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public sealed class CensusController : ControllerBase
    {
        private readonly ICensusRepository _repository;
        private readonly ITextFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CensusController"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="filter">The text filter.</param>
        public CensusController(ICensusRepository repository, ITextFilter filter)
        {
            _repository = repository;
            _filter = filter;
        }

        /// <summary>
        /// Gets the specified page of the list of acts matching the
        /// specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Page.</returns>
        [HttpGet("api/acts")]
        [ProducesResponseType(200)]
        public ActionResult<DataPage<ActInfo>> GetActs(
            [FromQuery] ActFilterModel filter)
        {
            var page = _repository.GetActs(filter.GetFilter(_filter));
            return Ok(page);
        }
    }
}
