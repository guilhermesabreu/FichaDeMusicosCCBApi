namespace FichaDeMusicosCCB.Domain.ViewModels
{
    public class HinoViewModel
    {
        public int IdHino { get; set; }
        public int NumeroHino { get; set; }
        public string? VozHino { get; set; }
        public int IdOcorrencia { get; set; }
        public OcorrenciaViewModel Ocorrencia { get; set; }
    }
}