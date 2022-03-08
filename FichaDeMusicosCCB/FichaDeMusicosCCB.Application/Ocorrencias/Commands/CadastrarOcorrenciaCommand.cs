using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Ocorrencias.Commands
{
    public class CadastrarOcorrenciaCommand : IRequest<OcorrenciaViewModel>
    {
        public int NumeroLicao { get; set; }
        public string? NomeMetodo { get; set; }
        public string? ObservacaoInstrutor { get; set; }
        public string? DataOcorrencia { get; set; }
        public int IdPessoa { get; set; }
        public CadastrarOcorrenciaCommand(OcorrenciaInputModel input)
        {
            input.Adapt(this);
        }
    }
}
