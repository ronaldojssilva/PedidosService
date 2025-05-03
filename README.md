# 📦 PedidosService API

> Solução desenvolvida em .NET 8 com arquitetura escalável, robusta e orientada a testes, responsável por **receber, calcular impostos e disponibilizar pedidos** entre dois sistemas externos (Sistema A → Sistema B).

---

## 📚 Sumário

- [🚀 Visão Geral](#-visão-geral)
- [🏗️ Arquitetura](#️-arquitetura)
- [⚙️ Tecnologias Utilizadas](#️-tecnologias-utilizadas)
- [🐳 Execução com Docker](#-execução-com-docker)
- [🧪 Testes](#-testes)
- [📥 Endpoints da API](#-endpoints-da-api)
- [📊 Carga e Benchmark](#-carga-e-benchmark)
- [🗂 Estrutura de Pastas](#-estrutura-de-pastas)
- [📌 Boas Práticas e Qualidade](#-boas-práticas-e-qualidade)

---

## 🚀 Visão Geral

Esta aplicação processa **150 mil a 200 mil pedidos por dia**, recebendo JSONs com dados de pedido, produtos e cliente, realizando o **cálculo de impostos com feature flags**, armazenando em PostgreSQL e depois expondo os pedidos com impostos para o consumo de outro sistema.

---

## 🏗️ Arquitetura

- **Camadas isoladas**: `API`, `Domain`, `Application`, `Infrastructure`
- **Feature Flag**: Permite alternar regras de impostos
- **Logs**: Serilog para logs estruturados
- **Resiliência e testes**: Alta cobertura com testes unitários e integração
- **Orquestração via Docker Compose**

---

## ⚙️ Tecnologias Utilizadas

| Tecnologia           | Descrição                                      |
|----------------------|------------------------------------------------|
| .NET 8               | Plataforma principal                          |
| PostgreSQL           | Banco de dados relacional                     |
| Docker + Compose     | Orquestração local                            |
| Serilog              | Logs estruturados                             |
| XUnit + FluentAssertions + NSubstitute | Testes unitários             |
| TestContainers       | Testes de integração com banco real           |
| K6                   | Stress test e validação de performance        |
| FeatureToggle.NET    | Feature flags no domínio                      |

---

## 🐳 Execução com Docker

### 🔧 Pré-requisitos:
- Docker e Docker Compose instalados

### ▶️ Subir aplicação:

```bash
docker-compose up --build
```
---

## 🌐 Acesso à API:

Swagger: http://localhost:5000/swagger

API Base: http://localhost:5000/api/


## 🧪 Testes
✅ Executar testes unitários:
```bash
dotnet test tests/PedidosService.UnitTests
````

✅ Executar testes de integração:
```bash
dotnet test tests/PedidosService.IntegrationTests
````

🧪 Stress test com K6 (instale o K6):
```bash
k6 run testes/k6-performance-test.js
```


# 📥 Endpoints da API

## POST `/api/pedido`
**Descrição:** Recebe e processa pedido.  
**Status:** 201 (Criado), 400 (Requisição inválida), 500 (Erro interno)

---

## GET `/api/pedido/{pedidoId}`
**Descrição:** Consulta pedido com imposto.

---

## 🗂 Estrutura de Pastas

```bash
📁 src/
  ├── API/                # Camada que expõe os endpoints — aqui é onde a mágica começa!
  ├── Application/        # Regras de aplicação, onde mora a inteligência do sistema
  ├── Domain/             # A alma do projeto — entidades, agregados, interfaces, etc.
  ├── Infrastructure/     # Integrações com o mundo real (banco, fila, cache, etc.)
   tests/
  ├── UnitTests/          # Testes unitários — aqui o bicho pega nos detalhes!
  └── IntegrationTests/   # Testes de integração — pra ver se tudo conversa direito
```

## 📌 Boas Práticas e Qualidade
- ✅ SOLID
- ✅ Clean Architecture
- ✅ Testes com alta cobertura
- ✅ Respostas RESTful com status apropriados
- ✅ Observabilidade com logs estruturados
- ✅ Código desacoplado e extensível
- ✅ Benchmark validando volumetria diária esperada