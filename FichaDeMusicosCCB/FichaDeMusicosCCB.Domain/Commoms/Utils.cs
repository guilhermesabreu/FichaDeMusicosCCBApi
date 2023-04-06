using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FichaDeMusicosCCB.Domain.Commoms
{
    public static class Utils
    {
        public static string MensagemErro500Padrao { get; private set; } = "Ocorreu um erro inesperado!!! Favor contate o Encarregado Local da sua comum";


        public static string NomeParaCredencial(string nome)
        {
            string[] nomeSplit = nome.Split(' ');
            if (nomeSplit.Length < 2)
                throw new ArgumentException("É preciso cadastrar o nome completo");

            return nome.Split(' ').First() + nome.Split(' ').Last();
        }

        public static string DataString(DateTime? data)
        {
            if (data != null)
                return data.Value.Date.ToString("dd/MM/yyyy");

            else
                return "";

        }

        public static string SepararDataDaHora(DateTime? data)
        {
            if (data != null)
                return data.Value.Date.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));

            else
                return String.Empty;

        }
    }


}
