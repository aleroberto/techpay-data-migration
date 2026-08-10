# Data Migration

## Objetivo

Simular a migração de um sistema legado em .NET para uma nova versão com banco de dados remodelado, mantendo os sistemas em paralelo durante a transição.

## Estratégia

A migração será realizada em fases:

1. Preparação do novo banco.
2. Migração inicial dos dados.
3. Sincronização incremental.
4. Validação e reconciliação.
5. Cutover para o novo sistema.
6. Desativação segura do legado.

Durante a transição, o sistema legado permanece como sistema oficial até que os critérios de validação sejam atendidos.

## Arquitetura

```text
Sistema Legado
      |
      v
Migration Service (.NET)
      |
      v
Banco Novo
```

O `MigrationService` será responsável pela leitura, transformação, carga e validação dos dados.

## Consistência

A migração deverá considerar:

* idempotência;
* controle de duplicidade;
* tratamento de falhas;
* integridade dos relacionamentos;
* reconciliação entre origem e destino.

## Observabilidade

Serão acompanhados:

* quantidade de registros processados;
* registros com erro;
* divergências entre origem e destino;
* tempo de processamento;
* status da migração.

## Rollback

O rollback da aplicação poderá retornar ao sistema legado.

Para os dados, serão mantidos backups e controles de migração antes do cutover definitivo.

## Documentação

* [Modelo legado](docs/05-modelo-legado.md)
* [Modelo novo](docs/06-modelo-novo.md)
* [Mapping](docs/07-mapping.md)

## Tecnologias

* .NET 10
* C#
* SQL Server 2022
* Docker
* Microsoft.Data.SqlClient

## Status

Em desenvolvimento.
