# 03 — Premissas

## Objetivo

Registrar as principais hipóteses utilizadas para desenvolver o projeto, já que o case não fornece todas as informações necessárias.

## Premissas

* O sistema legado está em produção e continuará funcionando durante a migração.
* O sistema legado utiliza SQL Server.
* O novo sistema também utilizará SQL Server.
* Existem dados históricos que precisam ser migrados.
* Os registros possuem identificadores que permitem relacionar os dados entre os modelos.
* Nem todos os campos possuem correspondência direta entre o modelo antigo e o novo.
* Podem existir dados inconsistentes ou inválidos no legado.
* A migração inicial será realizada separadamente da sincronização incremental.
* A migração poderá ser executada em lotes e reprocessada.
* O processo deverá evitar duplicidades.
* Alterações poderão ocorrer no legado durante a transição.
* Exclusões também deverão ser consideradas.
* O sistema legado e o novo permanecerão em paralelo durante um período de transição.
* A estratégia de sincronização será definida posteriormente, após análise das alternativas.
* O projeto utilizará C#/.NET e SQL Server.
* O ambiente será reproduzido localmente utilizando Docker.
* Os dados utilizados serão fictícios.

## Pontos ainda não definidos

O case não informa:

* volume de dados;
* frequência das alterações;
* SLA de sincronização;
* regras de negócio completas;
* janela de cutover;
* RTO/RPO;
* integrações existentes;
* infraestrutura de produção.

Esses pontos serão tratados como perguntas ao cliente ou premissas específicas quando forem necessários.

## Observação

As premissas acima não representam decisões arquiteturais.

A estratégia de sincronização, reconciliação, cutover e rollback será definida nas próximas etapas do projeto.
