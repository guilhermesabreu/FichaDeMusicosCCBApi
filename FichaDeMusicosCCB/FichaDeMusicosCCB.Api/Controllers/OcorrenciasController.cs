using FichaDeMusicosCCB.Application.Alunos.Commands;
using FichaDeMusicosCCB.Application.Ocorrencias.Commands;
using FichaDeMusicosCCB.Domain.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/ocorrencias")]
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
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> CadastrarOcorrencia([FromBody] OcorrenciaInputModel input)
        {
            try
            {
                var comando = new CadastrarOcorrenciaCommand(input);
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
        public async Task<IActionResult> AtualizarOcorrencia([FromBody] OcorrenciaInputModel input)
        {
            try
            {
                var comando = new AtualizarOcorrenciaCommand(input);
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

        [HttpDelete("{id_ocorrencia}")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ExcluirOcorrencia(long id_ocorrencia)
        {
            try
            {
                var comando = new ExcluirOcorrenciaCommand(id_ocorrencia);
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