# Data Migration

## Objetivo

Simular a migração de um sistema legado em .NET para uma nova plataforma com banco de dados remodelado, mantendo os sistemas em paralelo durante a transição.

O projeto demonstra uma estratégia de migração controlada, incremental e idempotente, contemplando aspectos técnicos e operacionais necessários para uma eventual migração produtiva.

## Estratégia

A solução foi estruturada em fases:

1. Preparação e modelagem do novo banco.
2. Mapeamento entre os modelos legado e novo.
3. Migração inicial dos dados.
4. Sincronização incremental utilizando watermark.
5. Controle de idempotência e duplicidade.
6. Validação e reconciliação.
7. Estratégia de transição e definição de critérios de Go/No-Go.
8. Estratégia de rollback.
9. Desativação segura do sistema legado.

Durante a transição, o sistema legado permanece como sistema oficial até que os critérios de validação e aprovação sejam atendidos.

## Arquitetura

```text
                    SISTEMA LEGADO
                          |
                          v
                    +-----------+
                    | LegacyDb  |
                    +-----+-----+
                          |
                          | leitura incremental
                          v
                 +-------------------+
                 | MigrationService  |
                 |       .NET        |
                 +---------+---------+
                           |
                           | mapeamento / upsert
                           v
                    +-----------+
                    |   NewDb   |
                    +-----------+
                           |
                           v
                   MigrationControl
```

O `MigrationService` é responsável pela leitura dos dados do legado, aplicação do mapeamento, escrita no banco de destino e controle da sincronização incremental.

## Cenário implementado

O primeiro cenário implementado contempla a migração da entidade `Customers`.

A solução suporta:

* carga inicial;
* sincronização incremental;
* identificação de registros alterados por `UpdatedAt`;
* controle do último processamento por `LastSyncAt`;
* inserção de novos registros;
* atualização de registros existentes;
* execução repetível;
* proteção contra duplicidade;
* validação dos dados no destino.

O processo foi validado com dados de laboratório e demonstrou a migração inicial dos clientes, atualização incremental de um registro e execução posterior sem novas alterações.

## Consistência e Idempotência

A solução utiliza operações idempotentes no destino.

O processo pode ser executado novamente sem criar registros duplicados.

A sincronização incremental utiliza um watermark armazenado em `NewDb.dbo.MigrationControl`, permitindo processar somente registros alterados desde o último processamento.

Em uma implementação produtiva, a atualização do watermark e a persistência dos dados devem ser protegidas por transação ou mecanismo equivalente para evitar avanço indevido do controle em situações de falha parcial.

## Reconciliação

A estratégia de validação considera:

* quantidade de registros;
* identificadores;
* campos relevantes;
* registros inseridos;
* registros atualizados;
* registros ausentes;
* duplicidades.

A primeira etapa foi validada comparando os registros migrados entre origem e destino.

## Observabilidade

A solução define como indicadores de acompanhamento:

* quantidade de registros lidos;
* quantidade de registros inseridos;
* quantidade de registros atualizados;
* quantidade de erros;
* divergências de reconciliação;
* `LastSyncAt`;
* duração da execução.

Em um ambiente produtivo, essas informações podem ser integradas às ferramentas corporativas de monitoramento e alertas.

## Transição e Rollback

Durante a transição, o sistema legado permanece como sistema oficial.

A mudança para o novo sistema depende da aprovação dos critérios de Go/No-Go, incluindo:

* reconciliação dos dados;
* testes funcionais;
* validação das integrações;
* desempenho dentro do esperado;
* ausência de erros críticos;
* rollback testado.

Caso os critérios não sejam atendidos, o cutover deve ser interrompido.

O legado permanece preservado durante a transição para permitir o retorno à operação anterior. A desativação definitiva somente ocorre após período de estabilização, backup, retenção dos dados e verificação das dependências remanescentes.

## Documentação

A evolução da solução está documentada em etapas:

* [Contexto](docs/01-contexto.md)
* [Perguntas ao cliente](docs/02-perguntas-ao-cliente.md)
* [Premissas](docs/03-premissas.md)
* [Requisitos](docs/04-requisitos.md)
* [Modelo de dados legado](docs/06-modelo-dados-legado.md)
* [Modelo de dados novo](docs/06-modelo-novo.md)
* [Mapping](docs/07-mapping.md)
* [Arquitetura](docs/08-arquitetura.md)
* [Alternativas e trade-offs](docs/09-alternativas-e-trade-offs.md)
* [Migração inicial](docs/10-migracao-inicial.md)
* [Mapping — implementação](docs/11-mapping.md)
* [Migração inicial — implementação](docs/12-initial-migration.md)
* [Solução técnica consolidada](docs/data-migration-solution.md)

## Tecnologias

* .NET 10
* C#
* SQL Server 2022
* Docker
* Microsoft.Data.SqlClient

## Status

**Concluído.**

A primeira etapa da solução foi implementada, executada e validada em ambiente controlado, incluindo migração inicial, sincronização incremental, controle de watermark, idempotência e reconciliação.

Os demais cenários do case serão incorporados à mesma arquitetura e documentação conforme sua implementação.
