using FichaDeMusicosCCB.Application.Alunos.Commands;
using FichaDeMusicosCCB.Domain.InputModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [Route("api/v1/alunos")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AlunosController> _logger;

        public AlunosController(IMediator mediator, ILogger<AlunosController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarAluno([FromBody] AlunoInputModel input)
        {
            var comando = new CadastrarAlunoCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarAluno([FromBody] AlunoInputModel input)
        {
            var comando = new AtualizarAlunoCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpDelete("{id_aluno}")]
        public async Task<IActionResult> ExcluirAluno(long id_aluno)
        {
            var comando = new ExcluirAlunoCommand(id_aluno);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

    }
}