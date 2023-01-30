# [CSU 03] Gerenciar Produtos

**Sumário:** Produtor insere, remove, atualiza e visualiza os produtos comercializados.

**Ator Primário:** Produtor

**Pré-Condições:**
1. Estar logado no sistema.
2. Ser um produtor.

**Fluxo principal:**

1. O produtor solicita manter informações dos produtos no sistema.
2. O sistema apresenta para ele as opções disponíveis, sendo elas: inserir novo produto;alterar produto existente;consultar ou remover.
3. O produtor seleciona a opção desejada e o caso de uso termina.

**Fluxo Alternativo ( 3 ) Inserir novo produto:**

1. O sistema apresenta o formulário para cadastro.
2. O produtor insere as informações.
3. O sistema verifica se os campos são válidos, caso esteja tudo certo, insere no banco e o caso de uso termina