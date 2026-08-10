

p01 - (Janelas de Manutenção e Downtime) Existe uma janela de manutenção tolerável (ex.: madrugada de domingo) para a execução da carga inicial de migração (snapshot), ou o sistema legado precisa operar 24/7 sem nenhuma interrupção durante todo o processo?

Precisamos saber para decidir e entender se o sistema for 24/7, a carga inicial não poderá bloquear as tabelas legadas por muito tempo.
Vantagens e desvantagens: Se for possivel o desligarmos o sistema antigo, tiramos um snapshot de todo o banco de dados, jogamos tudo para o novo, fazemos as validações e ligamos o novo. É mais simples, barato e mais seguro, pois as informações estão 'congeladas' durante a cópia. No caso, de não possibilidade de paramos, a complexidade e o custo crescem, pois serás necessário uma abordagem para garantir sincronicidade máxima.

p02 - (Regras de Negócio e Normalização) Pergunta: Quais foram as regras de negócio específicas que motivaram a normalização e a divisão de tabelas no novo banco de dados? Existem regras que deixaram de ser suportadas ou que agora possuem validações mais restritivas?
Importante para identificarmos se tais e quais regras mudaram evita que dados históricos perfeitamente válidos no sistema antigo sejam rejeitados pelo novo schema durante a sincronização incremental. 
Motivação: Precisamos identificar se haverá "rejeição por regra de negócio" e para onde esses registros irão (ex.: tabela de pendências / Dead Letter).

p03 - (Janela de Transição) Qual é a estimativa de duração do período de transição em que o sistema antigo e o novo rodarão em paralelo? 
Motivação: Dessa forma, conseguiremos identificar se a convivência de sistema vai durar meses, dias ou semanas. A complexidade de manter a sincronização bidirecional ou unidirecional muda drasticamente em diferentes casos.

p04 - (Convivência em Paralelo) Durante a fase em que os sistema coexistirão, qual sistema deverá ser o oficial para cada módulo financeiro?  
Motivação e impacto: A definição de qual sistema será o principal ajuda a evitar conflitos de concorrência; Onde uma alteração no sistema novo pudesse ser sobrescrita por um job legado/desatualizado.


p05 - (Integrações Externas e Contratos de API) Os parceiros externos e gateways de pagamento que integram com o nosso ecossistema, consomem APIs diretas do nosso banco/sistema ou utilizam contratos versionados? Eles estão preparados para mudar os endpoints durante a fase de transição?
Entendimento/Alinhamento/Motivação: Caso, continuarem escrevendo no formato antigo, precisarmos de uma camada de tradução na borda, capturar e adptar os payloads para o nosso novo modelo de dados.

p096 - (Volumetria e Janela de Sincronização Incremental) Qual é o volume diário médio de transações (INSERTs, UPDATEs) na base SQL Server legada e qual é o lag (atraso) máximo tolerável para que uma transação realizada no legado reflita no banco novo?
Vantagens/Desvantagens: Se o lag for de segundos, a adoção de um modelo de CDC exigirá recursos de monitoramento de transações através do log do banco de dados e complementarmente um serviços de fila/mensageria, adaptar o sistema para consumir estes serviços, lidar com falhas e garantir a idempotência das transações, contudo a precisão dos dados é quase cirúrgica. Para o caso onde o lag pode ser maior, a complexidade diminui ao rodar job agendados para o tempo determinado (por exemplo 1 hora atrás), a implementação é simplificada ao criar-se uma consulta (que apesar de custosa e talvez pesada) de SELECT no banco buscando por registros alterados no período de tempo correspondente. A principal desvantagem é que a latência está relacionada ao tempo determinado para esta atualização.

p06 - (Estratégia de Rollback) Em um cenário de desastre após a transição, qual são:
1 - o limite de perda de dados aceitavel: Determinar como o rollback de dados precisará lidar com o estado dos dados é determinante, para preparação da estratégia em ambos os cados: se será a partir de transações executadas no sistema novo ou se a partir da base antiga, que foi mantida em modo somente leitura durante a transição, basta para retomar a operação.
2 - o limite de tempo em que o sistema pode ficar parado: A partir dessa estimativa, poderemos saber como empregar a capacidade do time.