using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;


namespace CompiladorAntrWinui.Gramatica
{
    public class CustomErrorListener : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        private readonly List<string> _errors = new List<string>();

        public void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            int offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            string error = $"Erro léxico na linha {line}, posição {charPositionInLine}: {msg}";
            _errors.Add(error);
            output?.WriteLine(error);
        }

        public void SyntaxError(
            TextWriter output,
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            string error = $"Erro sintático na linha {line}, posição {charPositionInLine}: {msg}";
            _errors.Add(error);
            output?.WriteLine(error);
        }

        public bool HasErrors => _errors.Count > 0;

        public string GetFormattedErrors()
        {
            return string.Join(Environment.NewLine, _errors);
        }
    }
}
