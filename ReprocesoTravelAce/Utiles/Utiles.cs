using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReprocesoTravelAce.Utiles
{
    public class Utiles
    {

        public string Codificar(string texto)
        {
            // Encoding
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(texto);
            string pass = System.Convert.ToBase64String(plainTextBytes);
            return pass;
        }

        public string Decodificar(string texto)
        {
            // Normal
            var encodedTextBytes = Convert.FromBase64String(texto);
            string plainText = Encoding.UTF8.GetString(encodedTextBytes);
            return plainText;
        }


    }
}
