using System;
using System.Text;
using System.Collections.Generic;
namespace TokenOperador
{
    public enum TipoToken
    {
        Digito,
        Suma,
        Resta,
        Multiplicacion,
        Division,
        PuntoYComa,
        Espacio_Vacio,
        Desconocido
    }

    public enum Estado
    {
        Inicial,
        LeyendoDigito,
        Operador,
        Terminador,
        Error
    }

    public class Token
    {
        public TipoToken Tipo { get; private set; }
        public string Valor { get; private set; }
        public Token(TipoToken tipo, string valor)
        {
            Tipo = tipo;
            Valor = valor;
        }
        public override string ToString()
        {
            switch (Tipo)
            {
                case TipoToken.Digito: return "digito";
                case TipoToken.Suma: return "suma";
                case TipoToken.Resta: return "resta";
                case TipoToken.Multiplicacion: return "multiplicacion";
                case TipoToken.Division: return "division";
                case TipoToken.PuntoYComa: return "punto y coma";
                case TipoToken.Espacio_Vacio: return "espacio en blanco";
                default: return "desconocido";
            }
        }
    }

    public class AnalizadorLexico
    {
        private readonly Estado[,] matrizDeEstados;

        public AnalizadorLexico()
        {
            // Definir la matriz de estados
            // Filas: Estados actuales (Estado.Inicial, Estado.LeyendoDigito, etc.)
            // Columnas: Tipos de caracteres (0: dígito, 1: operador, 2: terminador, 3: otro)
            matrizDeEstados = new Estado[,]
            {
                //      Dígito               +\-\*\/          Terminador         Otro
                { Estado.LeyendoDigito, Estado.Operador, Estado.Terminador, Estado.Error },  // Estado.Inicial
                { Estado.LeyendoDigito, Estado.Operador, Estado.Terminador, Estado.Error },  // Estado.LeyendoDigito
                { Estado.LeyendoDigito, Estado.Operador, Estado.Terminador, Estado.Error },  // Estado.Operador
                { Estado.LeyendoDigito, Estado.Operador, Estado.Terminador, Estado.Error },  // Estado.Terminador
                { Estado.Error,         Estado.Error,    Estado.Error,      Estado.Error }   // Estado.Error
            };
        }

        public List<Token> AnalizarCadena(string cadena)
        {
            List<Token> tokens = new List<Token>();
            StringBuilder digitoActual = new StringBuilder();
            Estado estadoActual = Estado.Inicial;

            for (int i = 0; i < cadena.Length; i++)
            {
                char c = cadena[i];
                TipoToken tipoToken = ClasificarCaracter(c);
                int tipoCaracter = ObtenerTipoCaracter(tipoToken);

                // Transición de estado según la matriz
                estadoActual = matrizDeEstados[
                    (int)estadoActual,
                    tipoCaracter
                ];

                if (estadoActual == Estado.Error)
                {
                    // Manejar error (carácter no reconocido)
                    continue;
                }

                if (estadoActual == Estado.LeyendoDigito)
                {
                    if (tipoToken == TipoToken.Digito)
                    {
                        digitoActual.Append(c);
                        if (i == cadena.Length - 1 || !char.IsDigit(cadena[i + 1]))
                        {
                            tokens.Add(new Token(TipoToken.Digito, digitoActual.ToString()));
                            digitoActual.Clear();
                        }
                    }
                }
                else if (estadoActual == Estado.Operador || estadoActual == Estado.Terminador)
                {
                    if (tipoToken != TipoToken.Desconocido)
                    {
                        tokens.Add(new Token(tipoToken, c.ToString()));
                    }
                }
            }

            return tokens;
        }

        private TipoToken ClasificarCaracter(char c)
        {
            if (char.IsDigit(c))
                return TipoToken.Digito;
            switch (c)
            {
                case '+': return TipoToken.Suma;
                case '-': return TipoToken.Resta;
                case '*': return TipoToken.Multiplicacion;
                case '/': return TipoToken.Division;
                case ';': return TipoToken.PuntoYComa;
                case ' ': return TipoToken.Espacio_Vacio;
                default: return TipoToken.Desconocido;
            }
        }

        private int ObtenerTipoCaracter(TipoToken tipoToken)
        {
            switch (tipoToken)
            {
                case TipoToken.Digito:
                    return 0; // Dígito
                case TipoToken.Suma:
                case TipoToken.Resta:
                case TipoToken.Multiplicacion:
                case TipoToken.Division:
                    return 1; // Operador
                case TipoToken.PuntoYComa:
                case TipoToken.Espacio_Vacio:
                    return 2; // Terminador
                default:
                    return 3; // Otro (error)
            }
        }
    }
}