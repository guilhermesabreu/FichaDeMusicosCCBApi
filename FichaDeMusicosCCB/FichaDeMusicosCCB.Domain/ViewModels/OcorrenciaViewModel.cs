namespace FichaDeMusicosCCB.Domain.ViewModels
{
    public class OcorrenciaViewModel
    {
        public int IdOcorrencia { get; set; }
        public string? DataOcorrencia { get; set; }
        public int NumeroLicao { get; set; }
        public string? NomeMetodo { get; set; }
        public string? ObservacaoInstrutor { get; set; }
    }
}