# [CSU 05] Buscar produtos

**Sumário:** Consumidor faz busca por algum produto ou produtor

**Ator primário:** Consumidor

**Precondições:** 
1. Estar logado no sistema;
2. Ser um consumidor;
3. O produto precisa estar cadastrado no sistema;

**Fluxo Principal:**

1. O consumidor faz a busca por um produto cadastrado no sistema.
2. O sistema mostra os produtos disponíveis.

**Fluxo de exceção (1) Mensagem de alerta:**

1. Caso o produto pesquisado não esteja cadastrado no sistema, o software mostra uma mensagem acerca da indisponibilidade do alimento buscado.