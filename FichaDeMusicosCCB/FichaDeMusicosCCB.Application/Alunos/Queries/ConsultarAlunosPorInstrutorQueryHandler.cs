using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Query
{
    public class ConsultarAlunosPorInstrutorQueryHandler : IRequestHandler<ConsultarAlunosPorInstrutorQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarAlunosPorInstrutorQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarAlunosPorInstrutorQuery request, CancellationToken cancellationToken)
        {
            TypeAdapterConfig<FichaDeMusicosCCB.Domain.Entities.Pessoa, PessoaViewModel>.NewConfig();

            var alunos = _context.Pessoas.AsQueryable();
            var alunosPorInstrutor = alunos.Where(x => x.ApelidoInstrutorPessoa.Equals(request.ApelidoInstrutor));
            return alunosPorInstrutor.Adapt<List<PessoaViewModel>>();
        }

    }
}
