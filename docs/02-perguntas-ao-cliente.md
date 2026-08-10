# Perguntas ao Cliente

Este documento reúne as principais perguntas que precisam ser respondidas antes da definição da estratégia de migração e coexistência entre o sistema legado e a nova versão.

Cada pergunta possui sua respectiva motivação e impacto esperado na definição da solução.

---

## P01 - Janelas de Manutenção e Downtime

**Pergunta:** Existe uma janela de manutenção tolerável (ex.: madrugada de domingo) para a execução da carga inicial de migração (snapshot), ou o sistema legado precisa operar 24/7 sem nenhuma interrupção durante todo o processo?

**Motivação:** Precisamos saber isso para decidir a estratégia de migração inicial e entender se, caso o sistema precise operar 24/7, a carga inicial não poderá bloquear as tabelas legadas por muito tempo.

**Vantagens e desvantagens:** Se for possível desligar o sistema antigo, podemos realizar um snapshot de todo o banco de dados, transferir os dados para o novo ambiente, realizar as validações e, posteriormente, disponibilizar o novo sistema. Essa abordagem é mais simples, possui menor custo operacional e reduz a complexidade, pois as informações permanecem "congeladas" durante a cópia.

Caso não seja possível interromper o sistema, a complexidade e o custo da solução aumentam, pois será necessária uma estratégia para manter os dados do novo ambiente atualizados durante a execução da carga inicial.

---

## P02 - Regras de Negócio e Normalização

**Pergunta:** Quais foram as regras de negócio específicas que motivaram a normalização e a divisão de tabelas no novo banco de dados? Existem regras que deixaram de ser suportadas ou que agora possuem validações mais restritivas?

**Motivação:** É importante identificar quais regras foram alteradas para evitar que dados históricos perfeitamente válidos no sistema antigo sejam rejeitados pelo novo schema durante a sincronização incremental.

**Impacto:** Precisamos identificar se haverá "rejeição por regra de negócio" e definir como esses registros deverão ser tratados, por exemplo, por meio de uma tabela de pendências ou mecanismo equivalente.

---

## P03 - Janela de Transição

**Pergunta:** Qual é a estimativa de duração do período de transição em que o sistema antigo e o novo rodarão em paralelo?

**Motivação:** Dessa forma, conseguiremos identificar se a coexistência dos sistemas durará meses, dias ou semanas. A complexidade de manter a sincronização entre os sistemas pode mudar significativamente dependendo da duração desse período.

---

## P04 - Convivência em Paralelo

**Pergunta:** Durante a fase em que os sistemas coexistirão, qual sistema deverá ser o oficial para cada módulo financeiro?

**Motivação e impacto:** A definição de qual sistema será o principal ajuda a evitar conflitos de concorrência, nos quais uma alteração realizada no sistema novo poderia ser sobrescrita por um processo do sistema legado que ainda esteja operando com informações desatualizadas.

---

## P05 - Integrações Externas e Contratos de API

**Pergunta:** Os parceiros externos e gateways de pagamento que integram com o nosso ecossistema consomem APIs do sistema ou possuem algum tipo de acesso direto ao banco? Eles utilizam contratos de API versionados? Estão preparados para mudanças nos endpoints durante a fase de transição?

**Entendimento / Alinhamento / Motivação:** Caso as integrações continuem utilizando os contratos ou formatos antigos, poderá ser necessária uma camada de tradução ou compatibilidade na borda, responsável por adaptar os payloads e contratos utilizados externamente para o novo modelo interno.

---

## P06 - Volumetria e Janela de Sincronização Incremental

**Pergunta:** Qual é o volume diário médio de transações (INSERTs e UPDATEs) na base SQL Server legada e qual é o lag (atraso) máximo tolerável para que uma transação realizada no sistema legado seja refletida no banco novo?

**Vantagens / Desvantagens:** Se o lag aceitável for de poucos segundos, será necessária uma estratégia de sincronização de baixa latência, o que pode exigir mecanismos de captura de alterações e processamento assíncrono, além de monitoramento, tratamento de falhas e garantia de idempotência. Como vantagem, teremos uma menor janela de divergência entre os ambientes.

Caso o lag aceitável seja maior, a complexidade poderá ser reduzida por meio de processos agendados para períodos determinados. Uma possibilidade seria executar consultas buscando registros alterados dentro de uma janela de tempo específica. Apesar de ser uma implementação mais simples, essa abordagem pode gerar maior carga sobre o banco e sua principal desvantagem é que a latência estará relacionada ao intervalo definido para a atualização.

---

## P07 - Estratégia de Rollback

**Pergunta:** Em um cenário de desastre após a transição, quais são:

1. O limite de perda de dados aceitável?
2. O limite de tempo em que o sistema pode permanecer indisponível?

**Motivação:** Determinar o nível de perda de dados aceitável e o tempo máximo de indisponibilidade é fundamental para preparar a estratégia de rollback.

O limite de perda de dados ajudará a determinar como o rollback deverá lidar com o estado dos dados após a transição, considerando, por exemplo, se será necessário preservar e reaplicar as transações executadas no sistema novo ou se será possível retornar ao estado da base antiga.

O limite de tempo de indisponibilidade ajudará a determinar a estratégia operacional de recuperação e a capacidade necessária do time para executar o rollback dentro do período aceitável pelo negócio.
