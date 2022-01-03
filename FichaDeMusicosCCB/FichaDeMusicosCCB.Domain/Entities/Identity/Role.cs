using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FichaDeMusicosCCB.Domain.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}