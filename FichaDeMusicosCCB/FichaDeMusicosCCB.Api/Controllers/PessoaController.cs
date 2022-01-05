using FichaDeMusicosCCB.Application.Pessoas.Commands;
using FichaDeMusicosCCB.Domain.Entities.Identity;
using FichaDeMusicosCCB.Domain.InputModels;
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

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Ok("Logado com sucesso");
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPessoa([FromBody] PessoaInputModel input)
        {
            var comando = new CadastrarPessoaCommand(input);
            var result = await _mediator.Send(comando);
            return Ok(result);
        }

        [HttpPut()]
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