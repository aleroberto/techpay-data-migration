# 04 — Requisitos

## Objetivo

Definir o que a solução de migração precisa fazer.

## Requisitos funcionais

* Realizar a migração inicial do banco legado para o novo banco.
* Sincronizar alterações durante o período de transição.
* Evitar duplicidade durante migrações e reprocessamentos.
* Registrar erros e permitir reprocessamento.
* Comparar os dados dos dois bancos para identificar divergências.
* Permitir a operação dos sistemas em paralelo durante a transição.
* Definir qual sistema é o oficial em cada etapa.
* Possuir estratégia de cutover e rollback.
* Permitir a desativação segura do sistema legado.


## Requisitos não funcionais

* Consistência dos dados.
* Idempotência.
* Observabilidade.
* Execução local reproduzível com Docker.

## Restrições

* C# / .NET.
* SQL Server.
* Dados fictícios.
* Sistema legado e novo devem coexistir durante a transição.

## Critérios de sucesso

A solução deverá demonstrar:

1. Migração inicial funcionando.
2. Sincronização incremental funcionando.
3. Reprocessamento sem duplicidade.
4. Reconciliação entre os bancos.
5. Tratamento de erros.
6. Cutover e rollback documentados.

## Pontos em aberto

Ainda precisam ser definidos:

* volume de dados;
* frequência das alterações;
* SLA de sincronização;

Esses pontos serão definidos nas próximas etapas do projeto.
