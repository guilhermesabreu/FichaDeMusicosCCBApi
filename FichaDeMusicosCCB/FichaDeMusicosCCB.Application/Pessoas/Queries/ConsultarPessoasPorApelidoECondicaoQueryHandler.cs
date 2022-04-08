using FichaDeMusicosCCB.Domain.Commoms;
using FichaDeMusicosCCB.Domain.Entities;
using FichaDeMusicosCCB.Domain.ViewModels;
using FichaDeMusicosCCB.Persistence;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FichaDeMusicosCCB.Application.Pessoas.Query
{
    public class ConsultarPessoasPorApelidoECondicaoQueryHandler : IRequestHandler<ConsultarPessoasPorApelidoECondicaoQuery, List<PessoaViewModel>>
    {
        private readonly FichaDeMusicosCCBContext _context;
        public ConsultarPessoasPorApelidoECondicaoQueryHandler(FichaDeMusicosCCBContext context)
        {
            _context = context;
        }
        public async Task<List<PessoaViewModel>> Handle(ConsultarPessoasPorApelidoECondicaoQuery request, CancellationToken cancellationToken)
        {
            #region Mapeamento
            TypeAdapterConfig<Pessoa, PessoaViewModel>.NewConfig()
                    .Map(dest => dest.Id, src => src.IdPessoa)
                    .Map(dest => dest.Nome, src => src.NomePessoa)
                    .Map(dest => dest.ApelidoInstrutor, src => src.ApelidoInstrutorPessoa)
                    .Map(dest => dest.ApelidoEncarregado, src => src.ApelidoEncarregadoPessoa)
                    .Map(dest => dest.ApelidoEncRegional, src => src.ApelidoEncRegionalPessoa)
                    .Map(dest => dest.Regiao, src => src.RegiaoPessoa)
                    .Map(dest => dest.Regional, src => src.RegionalPessoa)
                    .Map(dest => dest.Celular, src => src.CelularPessoa)
                    .Map(dest => dest.Email, src => src.EmailPessoa)
                    .Map(dest => dest.DataNascimento, src => Utils.DataString(src.DataNascimentoPessoa))
                    .Map(dest => dest.DataInicio, src => Utils.DataString(src.DataInicioPessoa))
                    .Map(dest => dest.Comum, src => src.ComumPessoa)
                    .Map(dest => dest.Instrumento, src => src.InstrumentoPessoa)
                    .Map(dest => dest.Condicao, src => src.CondicaoPessoa);

            TypeAdapterConfig<Ocorrencia, OcorrenciaViewModel>.NewConfig()
                    .Map(dest => dest.DataOcorrencia, src => Utils.DataString(src.DataOcorrencia))
                    .Map(dest => dest.NumeroLicao, src => src.NumeroLicaoOcorrencia)
                    .Map(dest => dest.NomeMetodo, src => src.MetodoOcorrencia)
                    .Map(dest => dest.ObservacaoInstrutor, src => src.ObservacaoOcorrencia);

            //Observar se irá trazer os dados da ocorrência e hinos
            #endregion
            if (string.IsNullOrEmpty(request.ApelidoEncarregado) && string.IsNullOrEmpty(request.ApelidoEncarregadoRegional) && string.IsNullOrEmpty(request.ApelidoInstrutor))
                throw new ArgumentException("Informe o apelido do encarregado/instrutor ou encarregado regional.");

            if (string.IsNullOrEmpty(request.ApelidoEncarregado) && string.IsNullOrEmpty(request.ApelidoEncarregadoRegional) && string.IsNullOrEmpty(request.ApelidoInstrutor))
                throw new ArgumentException("Informe a condição que queira filtrar.");

            var pessoas = _context.Pessoas.AsQueryable().Include(x => x.Hinos).Include(x => x.Ocorrencias);
            var pessoasPorInstrutor = pessoas.Where(x => (x.ApelidoInstrutorPessoa.Equals(request.ApelidoInstrutor)
                                                      || x.ApelidoEncarregadoPessoa.Equals(request.ApelidoEncarregado)
                                                      || x.ApelidoEncRegionalPessoa.Equals(request.ApelidoEncarregadoRegional))
                                                      && x.CondicaoPessoa.Equals(request.Condicao)).ToList()
                .Select(x =>
                {
                    x.ApelidoEncarregadoPessoa = ObterNomePeloApelido(x.ApelidoEncarregadoPessoa).Result;
                    x.ApelidoInstrutorPessoa = ObterNomePeloApelido(x.ApelidoInstrutorPessoa).Result;
                    x.ApelidoEncRegionalPessoa = ObterNomePeloApelido(x.ApelidoEncRegionalPessoa).Result;
                    return x;
                }).ToList();
            return pessoasPorInstrutor.Adapt<List<PessoaViewModel>>();
        }

        public async Task<string> ObterNomePeloApelido(string apelido)
        {

            if (!string.IsNullOrEmpty(apelido) && apelido.Contains(";"))
            {
                var instrutores = apelido.Split(';');
                string nomesCompletos = "";
                foreach (var instrutor in instrutores)
                {
                    var pessoa = await _context.Pessoas.AsNoTracking()
                        .Include(x => x.User)
                        .Where(x => !string.IsNullOrEmpty(x.User.UserName) 
                        && x.User.UserName.Equals(instrutor)).FirstOrDefaultAsync();
                    if (pessoa != null)
                    {
                        nomesCompletos = !nomesCompletos.Equals("") 
                                        ? nomesCompletos + ";"+ pessoa.NomePessoa 
                                        : pessoa.NomePessoa;
                    }
                }
                return nomesCompletos;
            }
            else
            {

                var pessoa = await _context.Pessoas.AsNoTracking().Include(x => x.User).Where(x => !string.IsNullOrEmpty(x.User.UserName) && x.User.UserName.Equals(apelido)).FirstOrDefaultAsync();
                if (pessoa == null)
                    return string.Empty;

                return pessoa.NomePessoa;
            }

        }



    }
}
