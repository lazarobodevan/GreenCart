# [CSU 01] Fazer Cadastro

**Sumário:** Usuário cria uma conta na plataforma.

**Ator Primário:** Usuário

**Fluxo principal:**
1. O usuário solicita ao servidor a criação de uma nova conta;
2. O sistema disponibiliza os itens a serem preenchidos para o usuário;
3. O usuário insere nome, e-mail, senha e o tipo (Consumidor ou Produtor) e confirma os dados; 
4. O servidor autentica e grava o usuário no banco de dados e o caso de uso é finalizado.

**Fluxo de Exceção ( 3 ) Verificação de autenticidade dos dados:**

1. O sistema verifica se o e-mail do usuário já existe no banco de dados, e caso já exista, informa ao usuário um erro.
2. O sistema verifica também se o e-mail é válido e caso não seja é informado ao usuário de que este dado está incorreto.