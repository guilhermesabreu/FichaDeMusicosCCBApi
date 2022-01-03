using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;

namespace FichaDeMusicosCCB.Application.Alunos.Query
{
    public class ConsultarAlunosPorEncarregadoQueryHandler : IRequestHandler<ConsultarAlunosPorEncarregadoQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarAlunosPorEncarregadoQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarAlunosPorEncarregadoQuery request, CancellationToken cancellationToken)
        {
            TypeAdapterConfig<FichaDeMusicosCCB.Domain.Entities.Pessoa, PessoaViewModel>.NewConfig();

            var alunos = _context.Pessoas.AsQueryable();
            var alunosPorInstrutor = alunos.Where(x => x.ApelidoEncarregadoPessoa.Equals(request.ApelidoEncarregado));
            return alunosPorInstrutor.Adapt<List<PessoaViewModel>>();
        }

    }
}
