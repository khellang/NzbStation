using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NzbStation.Commands;
using NzbStation.Models;
using NzbStation.Queries;
using Zynapse;

namespace NzbStation.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MovieController : ControllerBase
    {
        public MovieController(ICommandDispatcher dispatcher, IQueryExecutor executor)
        {
            Dispatcher = dispatcher;
            Executor = executor;
        }

        private ICommandDispatcher Dispatcher { get; }

        private IQueryExecutor Executor { get; }

        [HttpGet]
        public async Task<ActionResult<PagedResultModel<MovieDetailsModel>>> GetAll([FromQuery] PagedQueryModel model, CancellationToken cancellationToken)
        {
            var query = new GetAllMoviesQuery(model.Page, model.Size);

            var result = await Executor.ExecuteAsync(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetailsModel>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new GetMovieQuery(id);

            var result = await Executor.ExecuteAsync(query, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<MovieDetailsModel>> Add([FromBody] AddMovieModel model, CancellationToken cancellationToken)
        {
            var command = new AddMovieCommand(model.Id.Value);

            var result = await Dispatcher.DispatchAsync(command, cancellationToken);

            if (result is null)
            {
                return NotFound();
            }

            var url = Url.Action(nameof(GetById), new { id = result.Id });

            return Created(url, result);
        }
    }
}
