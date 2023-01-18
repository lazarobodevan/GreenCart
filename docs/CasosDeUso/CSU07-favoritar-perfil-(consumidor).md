# [CSU 07] Favoritar perfil (Consumidor)

**Sumário:** Consumidor favorita o perfil de produtores

**Ator primário:** Consumidor

**Precondições:** O produtor precisa estar cadastrado no sistema

**Fluxo Principal:** 

1. O consumidor faz a busca por um produtor cadastrado no sistema.
2. O sistema mostra os perfis dos respectivos produtores.
3. O consumidor favorita o perfil do produtor.
4. O sistema salva o perfil do produtor na aba de favoritos.

**Fluxo Alternativo (1) Encomenda de produto:**

1. O consumidor faz a encomenda de algum alimento.
2. Após fechar o pedido o cliente favorita o perfil do produtor.
3. O sistema salva o perfil do produtor nos favoritos.

**Fluxo de exceção (1) Mensagem de alerta:**

1. Caso não haja nenhum produtor com o nome pesquisado pelo consumidor, o sistema exibe uma mensagem dizendo que não há nenhum perfil cadastrado com os respectivos dados.