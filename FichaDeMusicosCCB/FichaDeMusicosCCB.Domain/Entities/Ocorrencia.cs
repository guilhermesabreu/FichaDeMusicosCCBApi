namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Ocorrencia
    {
        public int IdOcorrencia { get; set; }
        public DateTime? DataOcorrencia { get; set; }
        public int NumeroLicaoOcorrencia { get; set; }
        public string? MetodoOcorrencia { get; set; }
        public string? ObservacaoOcorrencia { get; set; }
        public ICollection<PessoaOcorrencia> PessoaOcorrencias { get; set; }
        public ICollection<Hino> Hinos { get; set; }
    }
}