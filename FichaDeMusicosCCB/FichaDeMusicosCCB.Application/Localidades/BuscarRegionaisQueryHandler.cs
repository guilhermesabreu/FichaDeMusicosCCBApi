using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace FichaDeMusicosCCB.Application.Localidades.Query
{
    public class BuscarRegionaisQueryHandler : IRequestHandler<BuscarRegionaisQuery, List<string>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public BuscarRegionaisQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<string>> Handle(BuscarRegionaisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var resultado = await Regionais(request);
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

        public async Task<List<string>> Regionais(BuscarRegionaisQuery request)
        {
            return _context.Regionais.AsNoTracking().ToList()
                .Where(x => x.NomeRegional.ToUpper()
                .StartsWith(request.Input.ToUpper()))
                .Select(x => x.NomeRegional).ToList();
        }


    }
}
