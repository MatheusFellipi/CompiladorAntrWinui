using Antlr4.Runtime;
using System.Linq;
using System;

namespace CompiladorAntrWinui.Gramatica
{
    public class GramaticaVisitor : GramaticaBaseVisitor<string>
    {
        public override string VisitQuery(GramaticaParser.QueryContext context)
        {
            var selecao = Visit(context.selecao());
            var clausulas = context.GetRuleContexts<ParserRuleContext>()
                .Skip(1)
                .Select(Visit)
                .Where(result => !string.IsNullOrWhiteSpace(result));

            return $"{selecao} {string.Join(" ", clausulas)}".Trim();
        }

        public override string VisitSelecao(GramaticaParser.SelecaoContext context)
        {
            var entidade = Escape(context.IDENTIFICADOR().GetText());
            var condicao = context.condicao() != null ? Visit(context.condicao()) : "";
            return $"SELECT {entidade} {condicao}".Trim();
        }

        public override string VisitCondicao(GramaticaParser.CondicaoContext context)
        {
            var condicoes = context.condicao_completa()
                .Select(VisitCondicao_completa)
                .ToList();

            return string.Join(" AND ", condicoes);
        }

        public override string VisitCondicao_completa(GramaticaParser.Condicao_completaContext context)
        {
            var atributo = Escape(context.IDENTIFICADOR().GetText());
            var operador = context.operador().GetText();
            var valor = context.VALOR()?.GetText() ?? context.NUMERO()?.GetText();

            if (valor == null)
                throw new InvalidOperationException("A condição deve incluir um valor ou número válido.");

            if (operador == "igual a")
            {
                operador = "=";
            }
            else if (operador == "maior que")
            {
                operador = ">";
            }
            else if (operador == "menor que")
            {
                operador = "<";
            }

            return $"{atributo} {operador} {Escape(valor)}";
        }

        public override string VisitClausula_ordenacao(GramaticaParser.Clausula_ordenacaoContext context)
        {
            var atributo = Escape(context.IDENTIFICADOR().GetText());
            var direcao = context.ASC() != null ? "ASC" : context.DESC() != null ? "DESC" : "ASC";
            return $"ORDER BY {atributo} {direcao}";
        }

        public override string VisitClausula_agrupamento(GramaticaParser.Clausula_agrupamentoContext context)
        {
            var atributo = Escape(context.IDENTIFICADOR().GetText());
            return $"GROUP BY {atributo}";
        }

        public override string VisitClausula_agregacao(GramaticaParser.Clausula_agregacaoContext context)
        {
            if (context.CONTE() != null)
            {
                var entidade = Escape(context.IDENTIFICADOR().GetText());
                return $"COUNT({entidade})";
            }
            if (context.SOMAR() != null)
            {
                var atributo = Escape(context.IDENTIFICADOR().GetText());
                return $"SUM({atributo})";
            }
            return null;
        }

        private string Escape(string text)
        {
            return text.Contains(" ") ? $"`{text}`" : text;
        }
    }
}