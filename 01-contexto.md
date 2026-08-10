# Contexto

O sistema atual, desenvolvido em .NET, utiliza um banco de dados que será substituído por um novo modelo.

Como o sistema possui clientes em produção, a transição do modelo antigo para o novo deverá ocorrer de forma transparente, minimizando impactos aos clientes e evitando perda ou inconsistência de dados.

Durante o período de transição, o sistema atual e a nova versão deverão permanecer em utilização simultaneamente, até que seja realizada a transição definitiva para o novo modelo.

## Características do sistema atual

O sistema atual possui:

* integrações externas;
* clientes em produção;
* base de dados relacional.

## Características do novo modelo

O novo modelo de dados possui:

* divisão de tabelas;
* alteração de relacionamentos;
* renomeação ou substituição de campos;
* normalização de regras de negócio.

## Problema central

O desafio consiste em realizar a transição do modelo de dados atual para o novo modelo mantendo os sistemas em operação durante o período de coexistência, garantindo que os dados permaneçam íntegros e consistentes ao longo do processo.
