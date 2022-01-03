namespace FichaDeMusicosCCB.Domain.ViewModels
{
    public class PessoaOcorrenciaViewModel
    {
        public int IdPessoa { get; set; }
        public PessoaViewModel Pessoa { get; set; }
        public int IdOcorrencia { get; set; }
        public OcorrenciaViewModel Ocorrencia { get; set; }
    }
}