🚀 Sistema de Gerenciamento de Aluguel de Motos
📝 Descrição do Projeto
O Sistema de Gerenciamento de Aluguel de Motos é uma aplicação para gerenciar o aluguel de motos e o cadastro de entregadores. Ele permite a administração e a gestão de motos, além da locação e gerenciamento por entregadores.

🛠️ Tecnologias Utilizadas
Linguagem: C#
Framework: .NET (ASP.NET Core)
Banco de Dados: PostgreSQL
Sistema de Mensageria: RabbitMQ
Armazenamento de Arquivos: Local Disk
ORM: Entity Framework Core
Docker: Para containerização
✅ Requisitos Funcionais
🏍️ Cadastro e Gerenciamento de Motos
Cadastrar, Consultar, Modificar e Remover Motos
🏍️ Cadastro e Gerenciamento de Entregadores
Cadastrar Entregador e Atualizar Foto da CNH
🏍️ Locação de Motos
Alugar, Devolver e Consultar Valor das Motos
🐳 Como Rodar o Projeto com Docker
📋 Pré-requisitos
Certifique-se de ter o Docker e o Docker Compose instalados na sua máquina. Se ainda não os instalou, você pode baixá-los aqui.

🔧 Passos para Iniciar
Clone o Repositório

Primeiro, clone o repositório do projeto para sua máquina local. Abra um terminal e execute o seguinte comando:

bash
Copiar código
git clone <URL_DO_REPOSITORIO>
Em seguida, navegue até o diretório do projeto:

bash
Copiar código
cd <NOME_DIRETORIO_PROJETO>
Iniciar os Contêineres

O projeto utiliza o Docker Compose para gerenciar os contêineres. Execute o comando abaixo para construir as imagens e iniciar os contêineres:

bash
Copiar código
docker-compose up
Esse comando realiza as seguintes ações:

Baixa a imagem do PostgreSQL para o banco de dados.
Baixa a imagem do RabbitMQ para o sistema de mensageria.
Constrói e inicia a imagem da API usando o Dockerfile.
Configura as variáveis de ambiente necessárias, como usuário, senha e portas.
Verificar o Status dos Contêineres

Após o comando docker-compose up ser executado, você pode verificar o status dos contêineres com o comando:

bash
Copiar código
docker-compose ps
Acessar a Aplicação

API: A API estará disponível em http://localhost:8080.
RabbitMQ Management Console: O RabbitMQ estará disponível em http://localhost:15672 (Usuário: guest, Senha: guest).
PostgreSQL: O PostgreSQL estará disponível na porta 5432. Use a URL de conexão fornecida no arquivo docker-compose.yml para conectar-se ao banco de dados.
Visualizar o Código

Para verificar a estrutura do código e arquivos do projeto, abra o diretório do projeto em uma IDE, como o Visual Studio. Lembre-se de que os arquivos Dockerfile e docker-compose.yml estão localizados na raiz do projeto e não são exibidos diretamente na IDE, mas você pode visualizá-los no seu explorador de arquivos.
