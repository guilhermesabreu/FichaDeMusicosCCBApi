using FichaDeMusicosCCB.Application.Alunos.Query;
using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.QueryParameters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [Route("api/v1/alunos")]
    [ApiController]

    public class AlunosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PessoaController> _logger;

        public AlunosController(IMediator mediator, ILogger<PessoaController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("instrutor")]
        public async Task<IActionResult> ConsultarAlunosPorInstrutor([FromQuery] AlunoQueryParameter parameters)
        {
            var query = new ConsultarAlunosPorInstrutorQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("encarregado")]
        public async Task<IActionResult> ConsultarAlunosPorEncarregado([FromQuery] AlunoQueryParameter parameters)
        {
            var query = new ConsultarAlunosPorInstrutorQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("encarregado-regional")]
        public async Task<IActionResult> ConsultarAlunosPorEncarregadoRegional([FromQuery] AlunoQueryParameter parameters)
        {
            var query = new ConsultarAlunosPorInstrutorQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}