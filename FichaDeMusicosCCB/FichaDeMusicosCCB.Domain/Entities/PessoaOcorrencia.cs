namespace FichaDeMusicosCCB.Domain.Entities
{
    public class PessoaOcorrencia
    {
        public int IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; }
        public int IdOcorrencia { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
    }
}