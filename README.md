Sistema de Gerenciamento de Aluguel de Motos
Descrição do Projeto
O Sistema de Gerenciamento de Aluguel de Motos é uma aplicação para gerenciar o aluguel de motos e o cadastro de entregadores. Ele permite que administradores cadastrem, modifiquem e removam motos, e possibilita que entregadores se cadastrem, aluguem motos e gerenciem suas locações. A aplicação também gerencia eventos de cadastro de motos e assegura que apenas entregadores com habilitação apropriada possam alugar motos.

Tecnologias Utilizadas
Linguagem: C#
Framework: .NET (ASP.NET Core)
Banco de Dados: PostgreSQL
Sistema de Mensageria: RabbitMQ
Armazenamento de Arquivos: Local Disk
ORM: Entity Framework Core
Docker: Para containerização e gerenciamento de ambientes
Requisitos Funcionais
Cadastro de Moto (Admin)
Cadastrar Moto
Campos obrigatórios: Identificador, Ano, Modelo, Placa.
A placa deve ser única.
Após o cadastro, deve ser publicado um evento de moto cadastrada via mensageria.
Um consumidor deve ser configurado para notificar e armazenar motos do ano de 2024.
Consultar Motos
Permite a consulta e filtragem de motos existentes na plataforma pela placa.
Modificar Moto
Permite a alteração da placa de uma moto que foi cadastrada incorretamente.
Remover Moto
Permite a remoção de uma moto, desde que não haja registro de locações.
Cadastro de Entregador (Entregador)
Cadastrar Entregador
Campos obrigatórios: Identificador, Nome, CNPJ, Data de Nascimento, Número da CNH, Tipo da CNH, Imagem da CNH.
O CNPJ e o número da CNH devem ser únicos.
Os tipos de CNH válidos são "A", "B" ou "A+B".
Atualizar Foto da CNH
Permite o envio e atualização da foto da CNH.
O formato do arquivo deve ser PNG ou BMP.
A foto deve ser armazenada em um serviço de armazenamento (ex.: Amazon S3, MinIO) e não diretamente no banco de dados.
Locação de Motos (Entregador)
Alugar Moto
Planos disponíveis:
7 dias a R$30,00/dia
15 dias a R$28,00/dia
30 dias a R$22,00/dia
45 dias a R$20,00/dia
50 dias a R$18,00/dia
A locação deve ter uma data de início e uma data de término. A data de início é obrigatoriamente o primeiro dia após a criação.
Apenas entregadores com habilitação tipo "A" podem alugar motos.
Devolução de Moto e Consulta de Valor
Permite informar a data de devolução e consultar o valor total da locação.
Se a data de devolução for inferior à data prevista de término, será cobrada uma multa adicional:
Para o plano de 7 dias: 20% sobre o valor das diárias não efetivadas.
Para o plano de 15 dias: 40% sobre o valor das diárias não efetivadas.
Se a data de devolução for superior à data prevista de término, será cobrado um valor adicional de R$50,00 por diária adicional.
Como Rodar o Projeto com Docker
Pré-requisitos
Certifique-se de que você tem o Docker e o Docker Compose instalados na sua máquina. Você pode baixar e instalar o Docker e o Docker Compose a partir do site oficial do Docker.

Passos para Iniciar
Clone o Repositório

Primeiro, clone o repositório para sua máquina local:

bash
Copiar código
git clone <URL_DO_REPOSITORIO>
cd <NOME_DIRETORIO_PROJETO>
Configuração do Ambiente

O projeto já está configurado para usar Docker e Docker Compose. Não é necessário modificar os arquivos de configuração do Docker para iniciar o ambiente.

Iniciar os Contêineres

Execute o comando abaixo para construir as imagens e iniciar os contêineres:

bash
Copiar código
docker-compose up
Isso fará o Docker baixar as imagens necessárias, construir as imagens personalizadas e iniciar os contêineres para o serviço da API, o banco de dados PostgreSQL e o RabbitMQ.

Verificar o Status dos Contêineres

Após a execução do comando docker-compose up, você pode verificar o status dos contêineres com o comando:

bash
Copiar código
docker-compose ps
Acessar a Aplicação

A API estará disponível em http://localhost:8080.
O RabbitMQ Management Console estará disponível em http://localhost:15672 (Usuário: guest, Senha: guest).
O PostgreSQL estará disponível na porta 5432, e você pode conectar-se a ele usando a URL de conexão fornecida no Docker Compose.
Parar os Contêineres

Para parar e remover os contêineres, use o comando:

bash
Copiar código
docker-compose down
Estrutura dos Arquivos Docker
Dockerfile: Configura a imagem da API, especificando o ambiente de execução e as dependências necessárias.
docker-compose.yml: Define e gerencia os serviços, redes e volumes necessários para a aplicação.
Para mais detalhes sobre a configuração do Docker e como personalizar o ambiente, consulte os arquivos Dockerfile e docker-compose.yml na raiz do projeto.
