using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace FichaDeMusicosCCB.Application.Localidades.Query
{
    public class BuscarComunsQueryHandler : IRequestHandler<BuscarComunsQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarComunsQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarComunsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var resultado = await Comuns(request);
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

        public async Task<List<string>> Comuns(BuscarComunsQuery request)
        {
            return _context.Comuns.AsNoTracking().ToList()
                .Where(x => x.NomeComum.ToUpper()
                .StartsWith(request.Input.ToUpper()))
                .Select(x => x.NomeComum).ToList();
        }

    }
}
