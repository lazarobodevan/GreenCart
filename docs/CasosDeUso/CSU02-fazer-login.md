# [CSU02] Fazer Login

**Sumário:** Usuário autenticar-se na plataforma.

**Ator Primário:** Usuário

**Fluxo principal:**
1. O usuário solicita ao servidor sua autenticação com um e-mail e senha;
2. O serviço de autenticação verifica o e-mail e senha repassados, conferindo sua validade;
3. O serviço de autenticação verifica o tipo de usuário e o envia à interface correta;
4. O usuário consegue acessar os dados disponibilizados e o caso de uso é encerrado;
	
**Fluxo de Exceção ( 2 ) Verificação de autenticidade do e-mail:**

1. O sistema verifica se o e-mail do usuário não existe no banco de dados e se a senha condiz com o e-mail repassado, caso negativo envia uma mensagem de erro ao usuário;