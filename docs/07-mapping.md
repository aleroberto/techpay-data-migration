# 07 — Mapping

## Objetivo

Definir como os principais dados do modelo legado serão enviados para o novo modelo.

## Principais mapeamentos

| Origem                 | Destino                | Regra     |
| ---------------------- | ---------------------- | --------- |
| `Customers.CustomerId` | `Customers.CustomerId` | Mantido   |
| `Customers.Name`       | `Customers.Name`       | Mantido   |
| `Customers.Document`   | `Customers.Document`   | Mantido   |
| `Accounts.AccountType` | `Accounts.Type`        | Renomeado |
| `Accounts.Balance`     | `Accounts.Balance`     | Mantido   |
| `Transactions.Amount`  | `Transactions.Amount`  | Mantido   |

## Tratamento

* Campos obrigatórios não podem ser nulos.
* Registros inválidos devem ser registrados.
* Registros já processados não devem ser duplicados.
* Erros devem permitir reprocessamento.

## Observação

O mapping será utilizado pela migração inicial e pela sincronização incremental.
