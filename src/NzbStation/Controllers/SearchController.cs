using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NzbStation.Models;
using NzbStation.Queries;
using Zynapse;

namespace NzbStation.Controllers
{
    [ApiController]
    [Route("search")]
    public class SearchController : ControllerBase
    {
        public SearchController(IQueryExecutor executor)
        {
            Executor = executor;
        }

        private IQueryExecutor Executor { get; }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] SearchMoviesModel model, CancellationToken cancellationToken)
        {
            var query = new SearchMoviesQuery(model.Q, model.Page ?? 1);

            var result = await Executor.ExecuteAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}
