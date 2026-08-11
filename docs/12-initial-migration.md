# 12 — Migração inicial de Customers

## Objetivo

Implementar a primeira etapa da migração de dados entre o banco legado (`LegacyDb`) e o novo banco (`NewDb`).

## Fluxo

```text
LegacyDb.Customers
       ↓
CustomerRepository
       ↓
Customer
       ↓
CustomerMigrationService
       ↓
NewCustomerRepository
       ↓
NewDb.Customers
```

## Mapeamento

| LegacyDb   | NewDb       |
| ---------- | ----------- |
| CustomerId | CustomerId  |
| Name       | Name        |
| Document   | Document    |
| Email      | Email       |
| Status     | Status      |
| Address    | Não migrado |

O campo `Address` existe no modelo legado, mas não possui correspondente no novo modelo.

## Migração inicial

A primeira execução processou os três clientes existentes no banco legado:

```text
Clientes encontrados para sincronização: 3
Cliente processado: 1 - Joao Silva
Cliente processado: 2 - Maria Souza
Cliente processado: 3 - Carlos Lima
Sincronização concluída.
```

## Sincronização incremental

O banco legado recebeu o campo `UpdatedAt` para permitir a identificação de registros novos ou alterados no ambiente de laboratório.

O último processamento é armazenado em:

```text
NewDb.dbo.MigrationControl
```

O processo utiliza o campo `LastSyncAt` como watermark para buscar somente registros alterados após o último processamento.

Foi realizada uma alteração no cliente `1` no banco legado:

```text
Joao Silva → Joao Silva Atualizado
```

Após a execução da sincronização, o cliente foi atualizado corretamente no `NewDb`.

## Idempotência e duplicidade

O destino utiliza `Upsert`, permitindo inserir novos clientes ou atualizar clientes existentes.

Uma nova execução sem alterações retornou:

```text
Clientes encontrados para sincronização: 0
Sincronização concluída.
```

A validação de duplicidade retornou:

```text
CustomerId    Total
1             1
2             1
3             1
```

Isso demonstra que a execução repetida não criou registros duplicados.

## Resultado

A etapa está funcional, contemplando:

* leitura do banco legado;
* mapeamento entre os modelos;
* migração inicial;
* sincronização incremental;
* controle do último processamento;
* atualização de registros existentes;
* proteção contra duplicidade;
* execução repetível;
* validação dos dados migrados.

## Estratégia de transição

Durante a migração, o sistema legado permanece como sistema oficial.

O novo sistema recebe os dados em paralelo para validação. A troca do sistema oficial ocorre somente após a validação dos dados, testes funcionais e aprovação dos critérios de go/no-go.

## Consistência e falhas parciais

A sincronização utiliza operações idempotentes no destino.

O `LastSyncAt` somente deve avançar após o processamento dos registros. Para produção, recomenda-se proteger a gravação dos dados e a atualização do controle de sincronização por transação ou mecanismo equivalente, garantindo que o LastSyncAt não avance em caso de falha parcial.

Em caso de falha, o processamento pode ser reexecutado sem gerar duplicidades.

## Reconciliação

Após a migração, devem ser comparados entre origem e destino:

* quantidade de registros;
* identificadores;
* campos relevantes;
* registros inseridos, atualizados ou ausentes.

Diferenças devem ser corrigidas antes da mudança do sistema oficial.

## Observabilidade

A execução deve registrar:

* início e fim;
* quantidade de registros lidos;
* quantidade de registros inseridos e atualizados;
* erros;
* `LastSyncAt`;
* duração da execução.

Em produção, essas informações seriam integradas ao monitoramento corporativo e utilizadas para alertas.

## Critérios de Go/No-Go

A mudança para o novo sistema somente ocorre após:

* reconciliação dos dados aprovada;
* ausência de erros críticos;
* validação funcional;
* desempenho dentro do esperado;
* integrações externas validadas;
* rollback testado.

Caso algum critério crítico não seja atendido, a mudança é interrompida.

## Rollback

Durante a transição, o sistema legado permanece preservado como sistema oficial, permitindo retornar à versão anterior da aplicação.

O novo ambiente deve possuir controle dos lotes processados e mecanismos de restauração dos dados caso seja necessário retornar ao estado anterior.

O rollback deve ser testado antes do go-live.

## Desativação do legado

O legado não deve ser desativado imediatamente após a virada.

Após o novo sistema assumir como oficial, deve existir um período de estabilização e monitoramento.

Depois da aprovação do cliente, o legado pode ser colocado em modo somente leitura.

A desativação definitiva ocorre posteriormente, após backup, retenção dos dados e confirmação de que não existem dependências pendentes.

