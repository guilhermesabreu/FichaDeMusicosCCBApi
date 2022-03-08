namespace FichaDeMusicosCCB.Domain.InputModels
{
    public class OcorrenciaInputModel
    {
        public int IdOcorrencia { get; set; }
        public int NumeroLicao { get; set; }
        public string? NomeMetodo { get; set; }
        public string? ObservacaoInstrutor { get; set; }
        public string? DataOcorrencia { get; set; }
        public int IdPessoa { get; set; }
    }
}
