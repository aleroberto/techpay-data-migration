# 05 — Modelo Legado

## Objetivo

Representar de forma simples o banco existente antes da migração.

## Estrutura

```text
Customers
   │
   └── Accounts
          │
          └── Transactions
```

### Customers

| Campo      | Tipo    | Descrição     |
| ---------- | ------- | ------------- |
| CustomerId | INT     | Identificador |
| Name       | VARCHAR | Nome          |
| Document   | VARCHAR | Documento     |
| Email      | VARCHAR | E-mail        |
| Address    | VARCHAR | Endereço      |
| Status     | VARCHAR | Status        |

### Accounts

| Campo       | Tipo      | Descrição     |
| ----------- | --------- | ------------- |
| AccountId   | INT       | Identificador |
| CustomerId  | INT       | Cliente       |
| AccountType | VARCHAR   | Tipo          |
| Balance     | DECIMAL   | Saldo         |
| Status      | VARCHAR   | Status        |
| UpdatedAt   | DATETIME2 | Alteração     |

### Transactions

| Campo           | Tipo      | Descrição     |
| --------------- | --------- | ------------- |
| TransactionId   | BIGINT    | Identificador |
| AccountId       | INT       | Conta         |
| Type            | VARCHAR   | Tipo          |
| Amount          | DECIMAL   | Valor         |
| TransactionDate | DATETIME2 | Data          |
| Status          | VARCHAR   | Status        |

## Problemas

O modelo legado possui algumas características que serão alteradas no novo modelo:

* endereço junto aos dados do cliente;
* tipos e status armazenados como texto;
* estrutura de relacionamento diferente;
* algumas informações precisarão ser transformadas durante a migração.

## Premissa

Este é um modelo fictício criado para demonstrar o processo de migração.
