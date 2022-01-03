namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Hino
    {
        public int IdHino { get; set; }
        public int NumeroHino { get; set; }
        public string? VozHino { get; set; }
        public int IdOcorrencia { get; set; }
        public Ocorrencia Ocorrencia { get; set; }
    }
}