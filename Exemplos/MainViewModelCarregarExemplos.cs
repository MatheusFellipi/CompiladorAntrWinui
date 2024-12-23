using System.Collections.Generic;
using System.IO;
using System;

namespace CompiladorAntrWinui.Exemplos
{
    public class MainViewModelCarregarExemplos
    {
        public static List<Exemplo> CarregarExemplos()
        {
            var exemplos = new List<Exemplo>();

            string invalidoPath = @"C:\Users\mathe\source\repos\CompiladorAntrWinui\Exemplos\Invalida";
            string validoPath = @"C:\Users\mathe\source\repos\CompiladorAntrWinui\Exemplos\Validas";


            if (Directory.Exists(validoPath))
            {
                foreach (var file in Directory.GetFiles(validoPath, "*.txt"))
                {
                    exemplos.Add(new Exemplo
                    {
                        Nome = Path.GetFileName(file),
                        Tipo = "Válido",
                        Caminho = file
                    });
                }
            }
            else
            {
                Console.WriteLine($"O diretório de exemplos válidos não foi encontrado: {validoPath}");
            }

            if (Directory.Exists(invalidoPath))
            {
                foreach (var file in Directory.GetFiles(invalidoPath, "*.txt"))
                {
                    exemplos.Add(new Exemplo
                    {
                        Nome = Path.GetFileName(file),
                        Tipo = "Inválido",
                        Caminho = file
                    });
                }
            }
            else
            {
                Console.WriteLine($"O diretório de exemplos inválidos não foi encontrado: {invalidoPath}");
            }

            return exemplos;
        }
    }
}
