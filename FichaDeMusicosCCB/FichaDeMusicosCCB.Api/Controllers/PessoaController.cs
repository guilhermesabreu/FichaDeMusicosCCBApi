using FichaDeMusicosCCB.Application.Pessoas.Commands;
using FichaDeMusicosCCB.Application.Pessoas.Query;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.QueryParameters;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [Route("api/v1/pessoas")]
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
        public async Task<IActionResult> Login([FromBody] CredencialInputModel input)
        {
            var comando = new LogarPessoaCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpGet("por-instrutor")]
        public async Task<IActionResult> ConsultarPessoasPorInstrutor([FromQuery] PessoaQueryParameter parameters)
        {
            var query = new ConsultarPessoasPorInstrutorQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("por-encarregado")]
        public async Task<IActionResult> ConsultarPessoasPorEncarregado([FromQuery] PessoaQueryParameter parameters)
        {
            var query = new ConsultarPessoasPorEncarregadoQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("por-encarregado-regional")]
        public async Task<IActionResult> ConsultarPessoasPorEncarregadoRegional([FromQuery] PessoaQueryParameter parameters)
        {
            var query = new ConsultarPessoasPorEncarregadoRegionalQuery(parameters);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPessoa([FromBody] PessoaInputModel input)
        {
            var comando = new CadastrarPessoaCommand(input);
            var result = await _mediator.Send(comando);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarPessoa([FromBody] PessoaInputModel input)
        {
            var comando = new AtualizarPessoaCommand(input);
            var result = await _mediator.Send(comando);
            return Ok(result);
        }

        [HttpDelete("{user_name}")]
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