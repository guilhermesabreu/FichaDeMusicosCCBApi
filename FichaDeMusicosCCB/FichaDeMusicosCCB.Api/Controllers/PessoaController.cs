using FichaDeMusicosCCB.Application.Pessoas.Commands;
using FichaDeMusicosCCB.Application.Pessoas.Query;
using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.QueryParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/pessoas")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PessoaController> _logger;

        public PessoaController(IMediator mediator, ILogger<PessoaController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] CredencialInputModel input)
        {
            var comando = new LogarPessoaCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpGet("por-condicao")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ConsultarPessoasPorCondicao([FromQuery] PessoaQueryParameter parameters)
        {
            var query = new ConsultarPessoasPorApelidoECondicaoQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> AtualizarPessoa([FromBody] PessoaInputModel input)
        {
            var comando = new AtualizarPessoaCommand(input);
            var result = await _mediator.Send(comando);
            return Ok(result);
        }

        [HttpDelete("{user_name}")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ExcluirPessoa(string user_name)
        {
            var comando = new ExcluirPessoaCommand(user_name);
            var result = await _mediator.Send(comando);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}