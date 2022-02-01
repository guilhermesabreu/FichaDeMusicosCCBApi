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
            try
            {
                var comando = new CadastrarAlunoCommand(input);
                var response = await _mediator.Send(comando);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPut]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> AtualizarAluno([FromBody] AlunoInputModel input)
        {
            try
            {
                var comando = new AtualizarAlunoCommand(input);
                var response = await _mediator.Send(comando);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("{id_aluno}")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ExcluirAluno(long id_aluno)
        {
            try
            {
                var comando = new ExcluirAlunoCommand(id_aluno);
                var response = await _mediator.Send(comando);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }
}