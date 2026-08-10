# 08 — Arquitetura

## Objetivo

Definir uma arquitetura simples para migrar os dados do banco legado para o novo banco e mantê-los sincronizados durante a transição.

## Alternativas consideradas

### 1. Dual Write

A aplicação grava simultaneamente nos dois bancos.

**Vantagem:** baixa latência.

**Desvantagem:** aumenta o acoplamento e o risco de inconsistência.

### 2. CDC

Captura as alterações realizadas no banco legado.

**Vantagem:** permite uma sincronização mais próxima do tempo real.

**Desvantagem:** adiciona complexidade à solução.

### 3. Watermark

Um processo consulta periodicamente os registros alterados desde a última execução, utilizando `UpdatedAt`.

**Vantagem:** simples de implementar e controlar.

**Desvantagem:** existe um pequeno atraso entre os sistemas.

## Decisão

Para este projeto será utilizado **watermark baseado em `UpdatedAt`**.

A solução será:

```text
Legacy SQL Server
       │
       │ carga inicial
       ▼
Migration Service
       │
       ▼
New SQL Server

Legacy SQL Server
       │
       │ alterações
       ▼
Sync Service
       │
       ▼
New SQL Server
```

## Controle

O processo de sincronização deverá:

* armazenar o último ponto processado;
* buscar registros alterados;
* atualizar ou inserir no novo banco;
* evitar duplicidades;
* registrar erros;
* permitir reprocessamento.

## Justificativa

O watermark foi escolhido porque atende ao objetivo do projeto com menor complexidade de implementação.

CDC e Dual Write continuam sendo alternativas possíveis para um ambiente real, dependendo de requisitos de volume, latência e infraestrutura.
