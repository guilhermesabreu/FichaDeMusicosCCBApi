using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace FichaDeMusicosCCB.Application.Localidades.Query
{
    public class BuscarRegioesQueryHandler : IRequestHandler<BuscarRegioesQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarRegioesQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarRegioesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var resultado = await Regioes(request);
                return resultado;
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(Utils.MensagemErro500Padrao);
            }

        }

        public async Task<List<string>> Regioes(BuscarRegioesQuery request)
        {
            return _context.Regioes.AsNoTracking().ToList()
                .Where(x => x.NomeRegiao.ToUpper()
                .StartsWith(request.Input.ToUpper()))
                .Select(x => x.NomeRegiao).ToList();
        }

    }
}
