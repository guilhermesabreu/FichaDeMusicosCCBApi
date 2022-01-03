using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Query
{
    public class ConsultarAlunosPorEncarregadoRegionalQueryHandler : IRequestHandler<ConsultarAlunosPorEncarregadoRegionalQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarAlunosPorEncarregadoRegionalQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarAlunosPorEncarregadoRegionalQuery request, CancellationToken cancellationToken)
        {
            TypeAdapterConfig<FichaDeMusicosCCB.Domain.Entities.Pessoa, PessoaViewModel>.NewConfig();

            var alunos = _context.Pessoas.AsQueryable();
            var alunosPorInstrutor = alunos.Where(x => x.ApelidoEncRegionalPessoa.Equals(request.ApelidoEncarregadoRegional));
            return alunosPorInstrutor.Adapt<List<PessoaViewModel>>();
        }

    }
}
