using Antlr4.Runtime;
using System;

namespace CompiladorAntrWinui.Gramatica
{
    public class Compilar
    {
        public static string TraduzirParaSQL(string entrada, out string arvore)
        {
            var lexer = new GramaticaLexer(CharStreams.fromString(entrada));
            var errorListener = new CustomErrorListener();

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);

            var tokens = new CommonTokenStream(lexer);
            var parser = new GramaticaParser(tokens);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);

            var queryContext = parser.query();

            if (errorListener.HasErrors)
            {
                throw new Exception($"Erros encontrados:\n{errorListener.GetFormattedErrors()}");
            }

            arvore = queryContext.ToStringTree(parser);

            var visitor = new GramaticaVisitor();
            return visitor.Visit(queryContext);
        }
    }
}
