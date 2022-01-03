using FichaDeMusicosCCB.Domain.Entities.Identity;

namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Pessoa
    {
        public int IdPessoa { get; set; }
        public string? NomePessoa { get; set;}
        public string? ApelidoInstrutorPessoa { get; set; }
        public string? ApelidoEncarregadoPessoa { get; set; }
        public string? ApelidoEncRegionalPessoa { get; set; }
        public string? RegiaoPessoa { get; set; }
        public string? RegionalPessoa { get; set; }
        public string? CelularPessoa { get; set; }
        public string? EmailPessoa { get; set; }
        public DateTime? DataNascimentoPessoa { get; set; }
        public DateTime? DataInicioPessoa { get; set; }
        public string? ComumPessoa { get; set; }
        public string? InstrumentoPessoa { get; set; }
        public string? CondicaoPessoa { get; set; }
        public ICollection<PessoaOcorrencia> PessoaOcorrencias { get; set; }
        public int IdUser { get; set; }
        public User User { get; set; }
    }
}