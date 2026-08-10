# 10 — Migração Inicial

## Objetivo

Realizar a primeira carga dos dados do banco legado para o novo banco.

## Fluxo

```text
Legacy SQL Server
       │
       ▼
Migration Service
       │
       ▼
New SQL Server
```

## Processo

1. Ler os dados do banco legado.
2. Aplicar o mapping definido.
3. Validar os dados.
4. Gravar no novo banco.
5. Registrar o resultado do processamento.

## Controle

A migração deverá:

* processar os dados em lotes;
* evitar registros duplicados;
* registrar erros;
* permitir reprocessamento;
* manter o progresso do processamento.

## Resultado esperado

Ao final da migração inicial, os dados válidos do legado deverão estar disponíveis no novo banco.

Os registros que apresentarem erros deverão ser identificados para posterior tratamento.

## Validação

Após a carga, será realizada uma comparação entre os bancos para verificar:

* quantidade de registros;
* registros com erro;
* possíveis divergências.

A reconciliação será detalhada posteriormente no projeto.
