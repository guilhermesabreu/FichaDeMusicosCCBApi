namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Ocorrencia
    {
        public int IdOcorrencia { get; set; }
        public DateTime? DataOcorrencia { get; set; }
        public int NumeroLicaoOcorrencia { get; set; }
        public string? MetodoOcorrencia { get; set; }
        public string? ObservacaoOcorrencia { get; set; }
        public int IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; }
        
    }
}