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

        [HttpGet("despertador")]
        public async Task<IActionResult> Despertador()
        {
            return Ok();
        }

        [HttpGet("buscar-aluno")]
        public async Task<IActionResult> BuscarAluno([FromQuery] string? text)
        {
            try
            {
                var query = new BuscarAlunoQuery(text);
                var response = await _mediator.Send(query);
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

        [HttpGet("buscar-instrutor")]
        public async Task<IActionResult> BuscarInstrutor([FromQuery] string? text)
        {
            try
            {
                var query = new BuscarInstrutorQuery(text);
                var response = await _mediator.Send(query);
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

        [HttpGet("buscar-encarregado-local")]
        public async Task<IActionResult> BuscarEncarregadoLocal([FromQuery] string? text)
        {
            try
            {
                var query = new BuscarEncarregadoLocalQuery(text);
                var response = await _mediator.Send(query);
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

        [HttpGet("buscar-encarregado-regional")]
        public async Task<IActionResult> BuscarEncarregadoRegional([FromQuery] string? text)
        {
            try
            {
                var query = new BuscarEncarregadoRegionalQuery(text);
                var response = await _mediator.Send(query);
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

        [HttpGet("buscar-pessoa-logada")]
        public async Task<IActionResult> BuscarPessoaLogada([FromQuery] string? apelido)
        {
            try
            {
                var query = new BuscarPessoaLogadaQuery(apelido);
                var response = await _mediator.Send(query);
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

        [HttpPost]
        public async Task<IActionResult> CadastrarPessoa([FromBody] PessoaInputModel input)
        {
            try
            {
                var comando = new CadastrarPessoaCommand(input);
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

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] CredencialInputModel input)
        {
            try
            {
                var comando = new LogarPessoaCommand(input);
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

        [HttpGet("por-condicao")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ConsultarPessoasPorCondicao([FromQuery] PessoaQueryParameter parameters)
        {
            try
            {
                var query = new ConsultarPessoasPorApelidoECondicaoQuery(parameters);
                var response = await _mediator.Send(query);
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

        [HttpPut("incluir-aluno-na-ficha")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> IncluirAlunoNaFicha([FromBody] AlunoNaFichaInputModel input)
        {
            try
            {
                var comando = new IncluirAlunoNaFichaCommand(input);
                var result = await _mediator.Send(comando);
                return Ok(result);
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
        public async Task<IActionResult> AtualizarPessoa([FromBody] PessoaInputModel input)
        {
            try
            {
                var comando = new AtualizarPessoaCommand(input);
                var result = await _mediator.Send(comando);
                return Ok(result);
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

        [HttpDelete("{id_pessoa}")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ExcluirPessoa(long id_pessoa)
        {
            try
            {
                var comando = new ExcluirPessoaCommand(id_pessoa);
                var result = await _mediator.Send(comando);
                if (result)
                {
                    return Ok();
                }
                return StatusCode(500, "Internal Server Error");
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