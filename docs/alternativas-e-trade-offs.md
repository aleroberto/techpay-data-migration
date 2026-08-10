# 09 — Alternativas e Trade-offs

## Alternativas

### Dual Write

Permite manter os dois bancos atualizados pela aplicação.

**Não escolhido:** aumenta o acoplamento e pode gerar inconsistência caso uma das gravações falhe.

### CDC

Permite capturar alterações diretamente no banco.

**Não escolhido:** oferece menor latência, mas adiciona complexidade ao projeto.

### Watermark

Consulta periodicamente os registros alterados.

**Escolhido:** possui menor complexidade e é suficiente para demonstrar migração, sincronização, idempotência e reprocessamento.

## Trade-off

A principal desvantagem da solução escolhida é a existência de um pequeno atraso entre os bancos.

Para este projeto, esse atraso é aceitável em troca de uma implementação mais simples e controlável.
