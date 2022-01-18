using FichaDeMusicosCCB.Application.Hinos.Commands;
using FichaDeMusicosCCB.Domain.InputModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/hinos")]
    [ApiController]
    public class HinosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HinosController> _logger;

        public HinosController(IMediator mediator, ILogger<HinosController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> CadastrarHino([FromBody] HinoInputModel input)
        {
            var comando = new CadastrarHinoCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpPut]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> AtualizarHino([FromBody] HinoInputModel input)
        {
            var comando = new AtualizarHinoCommand(input);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

        [HttpDelete("{id_hino}")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> ExcluirHino(long id_hino)
        {
            var comando = new ExcluirHinoCommand(id_hino);
            var response = await _mediator.Send(comando);
            return Ok(response);
        }

    }
}