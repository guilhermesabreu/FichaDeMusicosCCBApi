namespace FichaDeMusicosCCB.Domain.Entities
{
    public class Regional
    {
        public int IdRegional { get; set; }
        public string NomeRegional { get; set; }
        public ICollection<Regiao> Regioes { get; set; }

    }
}