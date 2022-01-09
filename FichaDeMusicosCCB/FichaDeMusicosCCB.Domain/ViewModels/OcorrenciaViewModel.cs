namespace FichaDeMusicosCCB.Domain.ViewModels
{
    public class OcorrenciaViewModel
    {
        public int IdOcorrencia { get; set; }
        public string? DataOcorrencia { get; set; }
        public int NumeroLicaoOcorrencia { get; set; }
        public string? MetodoOcorrencia { get; set; }
        public string? ObservacaoOcorrencia { get; set; }
        public ICollection<HinoViewModel> Hinos { get; set; }
    }
}