Descrição do Projeto: Sistema de Gerenciamento de Aluguel de Motos
Objetivo
O objetivo deste projeto é desenvolver uma aplicação para gerenciar o aluguel de motos e o cadastro de entregadores. A aplicação permitirá que administradores cadastrem, modifiquem e removam motos, além de possibilitar que entregadores se cadastrem, aluguem motos e gerenciem suas locações. A aplicação deve também gerenciar eventos de cadastro de motos e assegurar que apenas entregadores com habilitação apropriada possam alugar motos.

Tecnologias Utilizadas
Linguagem: C#
Framework: .NET (ASP.NET Core)
Banco de Dados: PostgreSQL
Sistema de Mensageria: RabbitMQ 
Armazenamento de Arquivos: (Local Disk, Amazon S3, MinIO, etc.)
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
Requisitos Não Funcionais
Documentação

A documentação deve ser completa e clara, incluindo descrições de endpoints, parâmetros, e exemplos de requisições e respostas.
Documentar as regras de negócio e os casos de uso da aplicação.
Tratamento de Erros

Implementar tratamento de erros adequado para todas as operações.
Retornar mensagens de erro informativas e precisas.
Código Limpo e Organizado

Seguir boas práticas de codificação para garantir um código limpo e fácil de manter.
Arquitetura e Modelagem de Dados

Utilizar a arquitetura MVC e design patterns adequados para a organização do código.
Modelar dados de forma eficiente utilizando Entity Framework Core.
Docker e Docker Compose

Utilizar Docker para a containerização da aplicação e Docker Compose para gerenciar múltiplos containers, se necessário.
Incluir arquivos Dockerfile e docker-compose.yml no repositório.
Mensageria

Configurar e integrar um sistema de mensageria (ex.: RabbitMQ) para eventos e notificações.
