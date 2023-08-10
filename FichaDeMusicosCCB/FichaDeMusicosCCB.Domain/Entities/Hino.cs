namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Hino
    {
        public int IdHino { get; set; }
        public string NumeroHino { get; set; }
        public string? VozHino { get; set; }
        public DateTime? DataHino { get; set; }
        public int IdPessoa { get; set; }
        public Pessoa Pessoa { get; set; }
    }
}