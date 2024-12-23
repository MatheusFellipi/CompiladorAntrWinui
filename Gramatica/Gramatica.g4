grammar Gramatica;

SELECIONE: 'selecione';
QUE: 'que';
POSSUI: 'possui';
ORDENADO: 'ordenado';
POR: 'por';
AGRUPADO: 'agrupado';
CONTE: 'conte';
SOMAR: 'somar';

E: 'e'; 
OU: 'ou'; 
NAO: 'não' | 'nao'; 

ASC: 'ascendente';
DESC: 'descendente';

IDENTIFICADOR: [a-zA-Z_][a-zA-Z0-9_]*;
VALOR: '"' .*? '"';

NUMERO: [0-9]+;
ESPACO: [ \t\r\n]+ -> skip;

IGUAL: 'igual a' | '='; 
MAIOR: 'maior que' | '>'; 
MENOR: 'menor que' | '<'; 
DIFERENTE: 'diferente de' | '!='; 
CONTEM: 'contém' | 'contains'; 
COMECA: 'começa com' | 'startsWith'; 
MENOR_IGUAL: 'menor ou igual a' | '<='; 
MAIOR_IGUAL: 'maior ou igual a' | '>='; 

query: (selecao | clausula_agregacao) (clausula_ordenacao | clausula_agrupamento | clausula_condicional)* EOF;

clausula_agregacao: (CONTE IDENTIFICADOR | SOMAR IDENTIFICADOR);

clausula_ordenacao: ORDENADO POR IDENTIFICADOR (ASC | DESC)?;

selecao: SELECIONE IDENTIFICADOR (QUE POSSUI condicao)?;

condicao: condicao_completa (E condicao_completa)*;

condicao_completa: IDENTIFICADOR operador (VALOR | NUMERO) | NAO condicao_completa;

operador: IGUAL | MAIOR | MENOR | DIFERENTE | CONTEM | COMECA | MENOR_IGUAL | MAIOR_IGUAL;

clausula_agrupamento: AGRUPADO POR IDENTIFICADOR;

clausula_condicional: condicao_completa;