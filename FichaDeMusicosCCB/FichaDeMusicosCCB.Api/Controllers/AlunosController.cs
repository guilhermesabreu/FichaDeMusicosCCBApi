using FichaDeMusicosCCB.Application.Alunos.Commands;
using FichaDeMusicosCCB.Domain.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/alunos")]
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
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> CadastrarAluno([FromBody] AlunoInputModel input)
        {
            var comando = new CadastrarAlunoCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> AtualizarAluno([FromBody] AlunoInputModel input)
        {
            var comando = new AtualizarAlunoCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpDelete("{id_aluno}")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ExcluirAluno(long id_aluno)
        {
            var comando = new ExcluirAlunoCommand(id_aluno);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

    }
}