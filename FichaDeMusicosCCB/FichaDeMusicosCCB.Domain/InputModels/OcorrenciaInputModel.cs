namespace FichaDeMusicosCCB.Domain.InputModels
{
    public class OcorrenciaInputModel
    {
        public int IdOcorrencia { get; set; }
        public string? DataOcorrencia { get; set; }
        public int NumeroLicao { get; set; }
        public string? Metodo { get; set; }
        public string? Observacao { get; set; }
        public ICollection<HinoInputModel> Hinos { get; set; }
    }
}
