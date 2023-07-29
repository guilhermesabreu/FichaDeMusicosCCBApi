using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FichaDeMusicosCCB.Domain.QueryParameters
{
    public class BuscaLocalidadesQueryParameter
    {

        [Required(ErrorMessage = "Este campo é necessário", AllowEmptyStrings = false)]
        public string? Input { get; set; }
        public string? ApelidoPessoaLogada { get; set; }
    }
}
