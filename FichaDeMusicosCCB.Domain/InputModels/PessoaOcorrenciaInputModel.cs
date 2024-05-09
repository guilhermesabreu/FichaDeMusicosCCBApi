namespace FichaDeMusicosCCB.Domain.InputModels
{
    public class PessoaOcorrenciaInputModel
    {
        public int IdPessoa { get; set; }
        public PessoaInputModel Pessoa { get; set; }
        public int IdOcorrencia { get; set; }
        public OcorrenciaInputModel Ocorrencia { get; set; }
    }
}
