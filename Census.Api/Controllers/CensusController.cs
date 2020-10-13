using Census.Api.Models;
using Census.Core;
using Fusi.Tools.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Census.Api.Controllers
{
    /// <summary>
    /// Census data.
    /// </summary>
    /// <seealso cref="ControllerBase" />
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
        /// <param name="model">The filter model.</param>
        /// <returns>Page.</returns>
        [HttpGet("api/acts")]
        [ProducesResponseType(200)]
        public ActionResult<DataPage<ActInfo>> GetActs(
            [FromQuery] ActFilterModel model)
        {
            var page = _repository.GetActs(model.GetFilter(_filter));
            return Ok(page);
        }

        /// <summary>
        /// Gets the act with the specified ID.
        /// </summary>
        /// <param name="id">The act's identifier.</param>
        /// <returns>The act details.</returns>
        [HttpGet("api/acts/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Act> GetAct([FromRoute] int id)
        {
            Act act = _repository.GetAct(id);
            if (act == null) return NotFound();

            return Ok(act);
        }

        /// <summary>
        /// Lookup the specified number of items in the specified table.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Lookup items.</returns>
        [HttpGet("api/lookup")]
        [ProducesResponseType(200)]
        public ActionResult<IList<LookupItem>> GetLookupItems(
            [FromQuery] LookupModel model)
        {
            IList<LookupItem> result = _repository.Lookup(
                (DataEntityType)model.TableId,
                model.Filter,
                model.Limit);
            return Ok(result);
        }
    }
}
