using FichaDeMusicosCCB.Application.Alunos.Commands;
using FichaDeMusicosCCB.Application.Ocorrencias.Commands;
using FichaDeMusicosCCB.Domain.InputModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [Route("api/v1/ocorrencias")]
    [ApiController]
    public class OcorrenciasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OcorrenciasController> _logger;

        public OcorrenciasController(IMediator mediator, ILogger<OcorrenciasController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarOcorrencia([FromBody] OcorrenciaInputModel input)
        {
            var comando = new CadastrarOcorrenciaCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarOcorrencia([FromBody] OcorrenciaInputModel input)
        {
            var comando = new AtualizarOcorrenciaCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpDelete("{id_ocorrencia}")]
        public async Task<IActionResult> ExcluirOcorrencia(long id_ocorrencia)
        {
            var comando = new ExcluirOcorrenciaCommand(id_ocorrencia);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

    }
}