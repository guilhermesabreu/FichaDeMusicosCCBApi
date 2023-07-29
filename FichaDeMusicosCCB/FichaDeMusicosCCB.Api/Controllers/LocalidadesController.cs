using FichaDeMusicosCCB.Application.Hinos.Commands;
using FichaDeMusicosCCB.Application.Localidades.Query;
using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.QueryParameters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FichaDeMusicosCCB.Api.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/localidades")]
    [ApiController]
    public class LocalidadesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LocalidadesController> _logger;

        public LocalidadesController(IMediator mediator, ILogger<LocalidadesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("regionais")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> BuscarRegionais([FromQuery] BuscaLocalidadesQueryParameter parameters)
        {
            try
            {
                var query = new BuscarRegionaisQuery(parameters);
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

        [HttpGet("regioes")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> BuscarRegioes([FromQuery] BuscaLocalidadesQueryParameter parameters)
        {
            try
            {
                var query = new BuscarRegioesQuery(parameters);
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

        [HttpGet("comuns")]
        [Authorize(Roles = "ENCARREGADO,REGIONAL,INSTRUTOR")]
        public async Task<IActionResult> BuscarComum([FromQuery] BuscaLocalidadesQueryParameter parameters)
        {
            try
            {
                var query = new BuscarComunsQuery(parameters);
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

    }
}