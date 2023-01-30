# [CSU 08] Realizar Encomenda (Consumidor)

**Sumário:** Consumidor realiza a encomenda de algum produto ofertado por um produtor.

**Ator primário:** Consumidor

**Fluxo Principal:** 

1. O consumidor faz a busca por um produto cadastrado no sistema.
2. O sistema mostra os produtos encontrados.
3. O consumidor solicita a encomenda de uma quantidade daquele produto.
4. O sistema valida a quantidade, salva a encomenda e encerra o caso de uso.

**Fluxo de Exceção(4) Quantidade inválida:**

1. O consumidor insere uma quantidade superior à disponível, ou menor que zero.
2. O sistema exibe um alerta informando quantidade inválida e encerra o caso de uso.