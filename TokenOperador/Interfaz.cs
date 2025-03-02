using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenOperador
{
    public class Interfaz
    {
        private readonly AnalizadorLexico _analizador;

        public Interfaz()
        {
            _analizador = new AnalizadorLexico();
        }

        public void Iniciar()
        {
            while (true)
            {
                Console.Write("Introduce una expresión (o 'salir' para terminar): ");
                string entrada = Console.ReadLine();

                if (entrada?.ToLower() == "salir")
                    break;

                List<Token> tokens = _analizador.AnalizarCadena(entrada);
                string resultado = FormatearResultado(tokens);

                Console.WriteLine($"Resultado: [{resultado}]");
                Console.WriteLine();
            }
        }

        private string FormatearResultado(List<Token> tokens)
        {
            if (tokens.Count == 0)
                return "No se encontraron tokens";

            System.Text.StringBuilder resultado = new System.Text.StringBuilder();
            for (int i = 0; i < tokens.Count; i++)
            {
                resultado.Append(tokens[i].ToString());
                if (i < tokens.Count - 1)
                    resultado.Append(", ");
            }

            return resultado.ToString();
        }
    }
}