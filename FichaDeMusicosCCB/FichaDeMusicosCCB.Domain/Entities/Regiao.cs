namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Regiao
    {
        public int IdRegiao { get; set; }
        public string NomeRegiao { get; set; }
        public Regional Regional { get; set; }
        public ICollection<Comum> Comuns { get; set; }

    }
}