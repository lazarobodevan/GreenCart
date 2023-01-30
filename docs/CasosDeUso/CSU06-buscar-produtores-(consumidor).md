# [CSU 06] Buscar produtores

**Sumário:** Consumidor faz busca por algum produto ou produtor

**Ator primário:** Consumidor

**Precondições:**
1. Estar logado no sistema;
2. Ser um consumidor;
3. O produtor precisa estar cadastrado no sistema;

**Fluxo Principal:**

1. O consumidor faz a busca por um produtor cadastrado no sistema.
2. O sistema mostra os perfis dos respectivos produtores.

**Fluxo de exceção (1) Mensagem de alerta:**

1. Caso não haja nenhum produtor com o nome pesquisado pelo consumidor, o sistema exibe uma mensagem dizendo que não há nenhum perfil cadastrado com os respectivos dados.