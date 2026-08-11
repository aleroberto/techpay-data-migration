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
Mapeamento
       ↓
NewCustomer
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

## Idempotência

A inserção no `NewDb` utiliza `IF NOT EXISTS` considerando o `CustomerId`.

A migração foi executada duas vezes. A segunda execução não criou registros duplicados.

## Evidência

Carga inicial:

```text
Clientes encontrados no legado: 3
Cliente processado: 1 - Joao Silva
Cliente processado: 2 - Maria Souza
Cliente processado: 3 - Carlos Lima
Migração concluída.
```

Validação no `NewDb`:

```text
CustomerId    Name          Total
1             Joao Silva      1
2             Maria Souza     1
3             Carlos Lima     1
```

O resultado confirma que os três clientes foram migrados e que cada `CustomerId` possui apenas um registro no destino após uma segunda execução.

## Resultado

A primeira etapa da migração está funcional, contemplando:

* leitura do banco legado;
* mapeamento entre os modelos;
* escrita no banco de destino;
* proteção contra duplicidade;
* execução repetível;
* validação dos dados migrados.
