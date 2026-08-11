# TechPay — Data Migration

## Solução Técnica

## 1. Objetivo

Este documento apresenta a solução técnica desenvolvida para o case de migração de dados.

O objetivo é demonstrar uma estratégia segura para migração de um sistema legado para uma nova plataforma, considerando:

* migração inicial;
* sincronização incremental;
* integridade e consistência dos dados;
* idempotência;
* reconciliação;
* observabilidade;
* estratégia de transição;
* rollback;
* critérios de Go/No-Go;
* desativação segura do sistema legado.

A implementação utiliza .NET, SQL Server e Docker para reproduzir o cenário de migração em ambiente controlado.

---

# 2. Abordagem

A solução foi dividida em etapas para reduzir o risco da migração.

A primeira etapa concentra-se na migração da entidade `Customers`, estabelecendo a base técnica para posteriormente evoluir a solução para as demais entidades e cenários do case.

O princípio adotado foi manter o sistema legado como fonte oficial durante a transição e utilizar o novo ambiente para validação progressiva dos dados.

---

# 3. Arquitetura

```text
                   SISTEMA LEGADO
                         │
                         ▼
                  ┌──────────────┐
                  │   LegacyDb   │
                  │              │
                  │  Customers   │
                  └──────┬───────┘
                         │
                         │ leitura incremental
                         ▼
                ┌───────────────────┐
                │ MigrationService  │
                │                   │
                │ CustomerRepository│
                │       ↓           │
                │ CustomerMigration │
                │     Service       │
                │       ↓           │
                │ NewCustomer       │
                │ Repository        │
                └─────────┬─────────┘
                          │
                          │ Upsert
                          ▼
                  ┌──────────────┐
                  │    NewDb     │
                  │              │
                  │  Customers   │
                  │              │
                  │ Migration    │
                  │   Control    │
                  └──────────────┘
```

---

# 4. Cenário 1 — Migração de Customers

## 4.1 Migração inicial

A primeira etapa realizou a leitura dos clientes existentes no `LegacyDb` e sua gravação no `NewDb`.

O fluxo implementado foi:

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

Foram encontrados três clientes no banco legado.

Resultado:

```text
Clientes encontrados para sincronização: 3
Cliente processado: 1 - Joao Silva
Cliente processado: 2 - Maria Souza
Cliente processado: 3 - Carlos Lima
Sincronização concluída.
```

---

## 4.2 Mapeamento

| LegacyDb   | NewDb      | Observação                               |
| ---------- | ---------- | ---------------------------------------- |
| CustomerId | CustomerId | Mantido                                  |
| Name       | Name       | Mantido                                  |
| Document   | Document   | Mantido                                  |
| Email      | Email      | Mantido                                  |
| Status     | Status     | Mantido                                  |
| Address    | —          | Não possui correspondente no novo modelo |

O campo `Address` foi identificado no legado, porém não possui correspondente no modelo de destino e, portanto, não foi migrado.

---

# 5. Sincronização incremental

Para evitar a necessidade de realizar uma carga completa a cada execução, foi utilizado o campo `UpdatedAt` no legado.

O processo mantém o último ponto de sincronização na tabela:

```text
NewDb.dbo.MigrationControl
```

O campo `LastSyncAt` funciona como watermark.

O fluxo é:

```text
LastSyncAt
    ↓
LegacyDb
    ↓
UpdatedAt > LastSyncAt
    ↓
Registros alterados
    ↓
Upsert
    ↓
Novo LastSyncAt
```

Foi realizado um teste alterando:

```text
Joao Silva
      ↓
Joao Silva Atualizado
```

A execução identificou e atualizou corretamente o registro no `NewDb`.

---

# 6. Idempotência

O destino utiliza `Upsert`, permitindo:

* inserir registros inexistentes;
* atualizar registros existentes;
* executar novamente o processo sem criar duplicidades.

Após uma nova execução sem alterações, o resultado foi:

```text
Clientes encontrados para sincronização: 0
Sincronização concluída.
```

A validação de duplicidade apresentou:

```text
CustomerId    Total
1             1
2             1
3             1
```

Isso demonstra que a execução repetida não criou registros duplicados.

---

# 7. Consistência e falhas parciais

Durante a transição, o sistema legado permanece como fonte oficial.

O `LastSyncAt` somente deve avançar após o processamento dos registros.

Para produção, recomenda-se proteger a gravação dos dados e a atualização do controle de sincronização por transação ou mecanismo equivalente, garantindo que o watermark não avance em caso de falha parcial.

Em caso de erro, o lote pode ser reprocessado utilizando a estratégia idempotente.

---

# 8. Reconciliação

Após cada etapa de migração devem ser comparados entre origem e destino:

* quantidade de registros;
* identificadores;
* campos relevantes;
* registros inseridos;
* registros atualizados;
* registros ausentes.

Diferenças devem ser investigadas e corrigidas antes da promoção do novo sistema.

A validação realizada no primeiro cenário confirmou que os três `CustomerId` existentes no legado possuem exatamente um registro no destino.

---

# 9. Observabilidade

Em uma implementação produtiva, a execução deve registrar:

* início e fim da execução;
* quantidade de registros lidos;
* quantidade de registros inseridos;
* quantidade de registros atualizados;
* quantidade de erros;
* último `LastSyncAt`;
* duração da execução.

Essas informações podem ser integradas à solução corporativa de monitoramento e utilizadas para geração de alertas.

---

# 10. Estratégia de transição

Durante a migração, o sistema legado permanece como sistema oficial.

O novo sistema recebe os dados em paralelo para validação.

A troca do sistema oficial somente ocorre após:

1. validação dos dados;
2. testes funcionais;
3. reconciliação;
4. validação das integrações;
5. aprovação dos critérios de Go/No-Go.

---

# 11. Critérios de Go/No-Go

A mudança para o novo sistema somente deve ocorrer quando:

* a reconciliação dos dados estiver aprovada;
* não existirem erros críticos;
* os testes funcionais estiverem aprovados;
* o desempenho estiver dentro do esperado;
* as integrações externas estiverem validadas;
* o procedimento de rollback estiver testado.

Caso algum critério crítico não seja atendido, a mudança deve ser interrompida.

---

# 12. Rollback

Durante a transição, o legado permanece preservado como sistema oficial.

Isso permite retornar à operação anterior caso sejam identificados problemas durante a implantação do novo sistema.

O novo ambiente deve possuir controle dos lotes processados e mecanismos de restauração dos dados quando necessário.

O procedimento de rollback deve ser testado antes do Go-Live.

---

# 13. Desativação do legado

O sistema legado não deve ser desativado imediatamente após a virada.

Após o novo sistema assumir como oficial, deve existir um período de estabilização e monitoramento.

Após a aprovação do cliente:

1. o legado pode ser colocado em modo somente leitura;
2. deve ser mantido backup dos dados;
3. devem ser atendidos os requisitos de retenção;
4. devem ser verificadas dependências remanescentes;
5. somente então ocorre a desativação definitiva.

---

# 14. Estrutura do projeto

techpay-data-migration/
│
├── database/
│   ├── legacy/
│   │   ├── 01-create-tables.sql
│   │   └── 02-insert-data.sql
│   │
│   └── new/
│       ├── 01-create-tables.sql
│       └── 02-migration-control.sql
│
├── docker/
│   └── docker-compose.yml
│
├── docs/
│   ├── 01-contexto.md
│   ├── 02-perguntas-ao-cliente.md
│   ├── 03-premissas.md
│   ├── 04-requisitos.md
│   ├── 06-modelo-dados-legado.md
│   ├── 06-modelo-novo.md
│   ├── 07-mapping.md
│   ├── 08-arquitetura.md
│   ├── 09-alternativas-e-trade-offs.md
│   ├── 10-migracao-inicial.md
│   ├── 11-mapping.md
│   ├── 12-initial-migration.md
│   ├── 21-ai-usage.md
│   └── data-migration-solution.md
│
├── src/
│   ├── MigrationService/
│   │   ├── Data/
│   │   │   ├── Customer.cs
│   │   │   ├── CustomerMigrationService.cs
│   │   │   ├── CustomerRepository.cs
│   │   │   ├── LegacyDbConnection.cs
│   │   │   ├── NewCustomer.cs
│   │   │   ├── NewCustomerRepository.cs
│   │   │   └── NewDbConnection.cs
│   │   ├── MigrationService.csproj
│   │   └── Program.cs
│   │
│   └── SyncService/
│
├── tests/
├── .gitignore
└── readme.md



# 15. Evidências

As principais evidências da implementação do Cenário 1 incluem:

### Carga inicial

```text
Clientes encontrados para sincronização: 3
```

### Atualização incremental

```text
Cliente processado: 1 - Joao Silva Atualizado
```

### Execução sem alterações

```text
Clientes encontrados para sincronização: 0
Sincronização concluída.
```

### Validação de duplicidade

```text
CustomerId    Total
1             1
2             1
3             1
```

### Controle de sincronização

```text
EntityName    LastSyncAt
Customers     <timestamp>
```

Essas evidências demonstram a execução da carga inicial, atualização incremental, controle do watermark e comportamento idempotente.

---

# 16. Cenário 2

> A ser desenvolvido.

Nesta seção serão registradas:

* abordagem adotada;
* arquitetura;
* implementação;
* decisões técnicas;
* evidências;
* validações;
* riscos;
* estratégia de operação.

---

# 17. Conclusão

A primeira etapa demonstra uma estratégia de migração incremental e controlada, mantendo o legado como sistema oficial durante a transição.

A solução evita cargas completas desnecessárias, permite reprocessamento, reduz o risco de duplicidade e estabelece mecanismos de controle e validação que podem ser evoluídos para um cenário produtivo.

Os próximos cenários serão incorporados a este documento para apresentar a solução completa de forma consolidada.
