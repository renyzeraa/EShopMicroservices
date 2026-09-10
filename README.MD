# EShop Microservices

Projeto de referência de e-commerce em **.NET 10**, voltado ao estudo e à prática de arquitetura de microsserviços com **Vertical Slice Architecture** e **CQRS**.

## Serviços

| Serviço | Responsabilidade |
|---|---|
| Catalog.Api | Cadastro, consulta, atualização e exclusão de produtos. |
| Basket.Api | Armazenamento, consulta e exclusão de carrinhos de compras, com cache em Redis. |

Cada serviço possui seu próprio banco PostgreSQL. As APIs e suas dependências são orquestradas com Docker Compose.

## Tecnologias

| Área | Tecnologias |
|---|---|
| APIs | .NET 10, ASP.NET Core Minimal APIs e Carter |
| Comandos e consultas | MediatR |
| Validação e mapeamento | FluentValidation e Mapster |
| Persistência | PostgreSQL 17 e Marten |
| Cache | Redis |
| Monitoramento | Health checks e logging |
| Containers | Docker e Docker Compose |

## Arquitetura

- **Vertical Slice Architecture**: código organizado por caso de uso.
- **CQRS**: separação entre comandos de escrita e consultas de leitura.
- **BuildingBlocks**: componentes compartilhados de validação, logging e tratamento de erros.
- **Persistência independente**: cada microsserviço gerencia seus próprios dados.

## Estrutura

```text
src/
├── BuildingBlocks/            # Componentes compartilhados
├── Services/
│   ├── Catalog/               # Catálogo de produtos
│   └── Basket/                # Carrinho de compras
├── docker-compose.yml
└── docker-compose.override.yml
```

## Como rodar

Com o **Docker e o Docker Compose disponíveis no WSL**, execute os comandos no terminal Linux, a partir da raiz do repositório. Não é necessário instalar o SDK .NET localmente.

Para construir as imagens e iniciar as APIs, os bancos e o Redis:

```bash
cd src
docker compose up -d --build --wait
```

| API | HTTP | HTTPS |
|---|---|---|
| Catalog | http://localhost:6000 | https://localhost:6060 |
| Basket | http://localhost:6001 | https://localhost:6061 |

Acesse `/health` em cada API para verificar seu funcionamento.

Para parar todos os serviços, mantendo os containers e os dados:

```bash
docker compose stop
```

Para remover também os containers e a rede, use `docker compose down`. Evite `down -v`, pois ele apaga os volumes com os dados dos bancos.

## Licença

[MIT](LICENSE)
