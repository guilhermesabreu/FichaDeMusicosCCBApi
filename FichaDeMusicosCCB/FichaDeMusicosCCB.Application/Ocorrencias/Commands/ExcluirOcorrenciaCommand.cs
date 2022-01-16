using FichaDeMusicosCCB.Domain.InputModels;
using FichaDeMusicosCCB.Domain.ViewModels;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Ocorrencias.Commands
{
    public class ExcluirOcorrenciaCommand : IRequest<bool>
    {
        public long IdOcorrencia { get; set; }
        public ExcluirOcorrenciaCommand(long idOcorrencia)
        {
            IdOcorrencia = idOcorrencia;
        }
    }
}
