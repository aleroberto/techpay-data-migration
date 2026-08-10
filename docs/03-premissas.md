# Premissas do Projeto de Migração 

## 1. Objetivo
Este documento registra as premissas adotadas para o desenvolvimento do projeto de migração do sistema legado para a nova versão, conforme o cenário apresentado. As premissas existem para preencher lacunas de informação do case e permitir a evolução do projeto sem antecipar decisões arquiteturais.

---

## 2. Premissas sobre o cenário
- **P-001 — Existência de dois sistemas:** O sistema legado e o novo sistema estarão disponíveis simultaneamente durante um período de transição.
- **P-002 — O sistema legado está em produção:** Possui dados reais e não pode ser interrompido; a estratégia deve minimizar indisponibilidade e risco.
- **P-003 — O banco legado é relacional:** Utiliza SQL Server; alinhado ao requisito do case.
- **P-004 — O novo sistema também utilizará SQL Server:** O novo modelo será implementado em SQL Server para reduzir variáveis.

## 3. Premissas sobre os dados
- **P-005 — Os dados possuem identificadores:** Possuem IDs únicos para relacionamento entre modelos.
- **P-006 — Existem dados históricos:** A base contém históricos que devem ser considerados na migração.
- **P-007 — Nem todos os campos possuem correspondência direta:** Requer mapeamento explícito (renomeação, transformação, etc.).

## 4. Premissas sobre a migração
- **P-008 — A migração inicial será separada da sincronização incremental:** Etapas distintas.
- **P-009 — A migração poderá ser executada em lotes:** Processamento em batches.
- **P-011 — A migração poderá ser reexecutada:** Processo deve permitir controle de progresso e reprocessamento seguro.
- **P-012 — A migração deverá ser idempotente:** Execuções repetidas não devem gerar duplicidade.

## 5. Premissas sobre sincronização
- **P-013 — Alterações poderão ocorrer durante a transição:** O legado continua recebendo operações.
- **P-014 — A estratégia de sincronização ainda não está definida:** Avaliada na etapa de arquitetura.
- **P-015 — Exclusões deverão ser consideradas:** Estratégia de reflexão de exclusões a definir.

## 6. Premissas sobre consistência
- **P-016 — Consistência deverá ser mensurável:** Mecanismos para identificar divergências.
- **P-017 — Nem toda divergência será necessariamente crítica:** Classificação por severidade necessária.
- **P-018 — Reconciliação será parte do processo:** Comparação de dados críticos e regras de negócio.

## 7. Premissas sobre disponibilidade
- **P-019 — O sistema legado não poderá sofrer indisponibilidade prolongada:** Impacto mínimo, janela para cutover se necessária.
- **P-020 — O cutover será uma etapa controlada:** Exige validações e critérios objetivos.

## 8. Premissas sobre rollback
- **P-021 — Rollback deverá ser planejado antes do cutover:** Documentado e testado.
- **P-022 — Aplicação e dados deverão ser considerados:** Considerar estado pós-cutover.
- **P-023 — Dados não poderão ser descartados silenciosamente:** Estratégia definida antes da reversão.

## 9. Premissas sobre segurança
- **P-024 — Acesso ao banco deverá utilizar menor privilégio:** Permissões estritamente necessárias.
- **P-025 — Credenciais não serão armazenadas no código:** Uso de gerenciamento de secrets.
- **P-026 — Dados sensíveis não deverão aparecer nos logs:** Rastreabilidade sem exposição de dados.

## 10. Premissas sobre observabilidade
- **P-027 — Os processos de migração deverão ser observáveis:** Monitoramento de registros, erros, falhas e atrasos.
- **P-028 — Cada operação relevante deverá possuir rastreabilidade:** Uso de correlation ID.

## 11. Premissas sobre testes
- **P-029 — A solução será validada de forma automatizada:** Testes unitários, de integração, migração, reconciliação, etc.
- **P-030 — O ambiente de testes deverá ser reproduzível:** Uso de ferramentas como Docker.

## 12. Premissas sobre tecnologia
- **P-031 — Ecossistema .NET:** C# e ASP.NET Core.
- **P-032 — SQL Server:** Banco padrão.
- **P-033 — Entity Framework Core:** ORM preferencial, com acesso direto quando necessário.
- **P-034 — Containerização:** Componentes disponíveis via Docker.

## 13. Premissas sobre o ambiente do projeto
- **P-035 — O projeto será uma simulação controlada:** Implementação de portfólio.
- **P-036 — Dados fictícios:** Dados corporativos plausíveis, sem uso de dados reais.
- **P-037 — Escala será definida para demonstração:** Escala focada em validar comportamentos técnicos.

## 14. Limitações conhecidas
As seguintes informações não estão disponíveis e serão tratadas como perguntas ou premissas de simulação:
- Volume real, número de clientes/transações, taxas de crescimento, SLAs, janelas de cutover, RTO/RPO, regras de negócio completas, detalhes de integrações, requisitos regulatórios, infraestrutura de produção e auditoria/backup.

## 15. Premissas que exigem validação
As premissas com impacto alto incluem: P-003, P-005, P-007, P-013, P-015, P-019, P-021, P-027.

## 16. Relação com as próximas etapas
Entrada para requisitos, modelagem, mappings, arquitetura e critérios de operação.

## 17. Conclusão
Este documento estabelece o cenário técnico controlado, mantendo a diferença entre fatos, hipóteses e decisões de engenharia.