using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FichaDeMusicosCCB.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public override int Id { get; set; }
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public List<UserRole> UserRoles { get; set; }
        public string Role { get; set; }
        public Pessoa Pessoa { get; set; }
    }
}