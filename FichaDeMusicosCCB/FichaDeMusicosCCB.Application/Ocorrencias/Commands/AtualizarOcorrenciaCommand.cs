using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Ocorrencias.Commands
{
    public class AtualizarOcorrenciaCommand : IRequest<OcorrenciaViewModel>
    {
        public int IdOcorrencia { get; set; }
        public int NumeroLicao { get; set; }
        public string? NomeMetodo { get; set; }
        public string? ObservacaoInstrutor { get; set; }
        public int IdPessoa { get; set; }
        public AtualizarOcorrenciaCommand(OcorrenciaInputModel input)
        {
            input.Adapt(this);
        }
    }
}
