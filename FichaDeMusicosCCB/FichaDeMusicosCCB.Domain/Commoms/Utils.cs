using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FichaDeMusicosCCB.Domain.Commoms
{
    public static class Utils
    {
        public static string MensagemErro500Padrao { get; private set; } = "Ocorreu um erro inesperado!!! Favor contate a equipe de engenharia de software. (13)97405-8807";


        public static string NomeParaCredencial(string nome)
        {
            string[] nomeSplit = nome.Split(' ');
            if (nomeSplit.Length < 2)
                throw new ArgumentException("É preciso cadastrar o nome completo");

            return nome.Split(' ').First() + nome.Split(' ').Last();
        }
    }

    
}
