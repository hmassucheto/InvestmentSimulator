Simulador de Investimento em CDB

Este projeto simula uma aplicação de CDB com taxa de CDI fixa, retornando o valor bruto e líquido ao final de um prazo em meses. 
O frontend é feito em Angular e o backend em ASP.NET Core 8, utilizando princípios SOLID e testes unitários.

Variáveis fixas para simulação
- CDI: 0.9% ao mês (0.009)
- TB: 108% (1.08)

Imposto de Renda:
- Até 6 meses: 22.5%
- Até 12 meses: 20%
- Até 24 meses: 17.5%
- Acima de 24 meses: 15%

Este README explica como fazer:

- **Build & Deploy** da solução InvestmentSimulator via Docker e Docker Compose no Windows, sem pré-build local.  
- **Execução isolada** dos projetos backend (.NET 8.0) e frontend (Angular).  
- **Execução dos testes unitários** para backend

---

## Pré-requisitos

1. **Windows 10/11** com suporte a contêineres Linux  
2. **Docker Desktop** (versão mínima 4.0) ou **Rancher Desktop** instalado  
   - Docker Engine (versão mínima 20.10) e Docker Compose (versão mínima 2.0)  
   - Após a instalação, inicie o Docker Desktop (ou Rancher Desktop)
3. **Conexão com a internet**  
   - Para fazer pull das imagens base de .NET e Node.js  

---

## Estrutura de pastas

```
├── Dockerfile
├── docker-compose.yml
├── InvestmentSimulator.Api/
│   └── InvestmentSimulator.Api.csproj
├── InvestmentSimulator.Domain/
│   └── InvestmentSimulator.Domain.csproj
├── InvestmentSimulator.Tests/
│   └── InvestmentSimulator.Tests.csproj
└── InvestmentSimulator.Web/
    ├── package.json
    ├── angular.json
    └── src/
```

- **Dockerfile**: multi-stage para build do Angular e .NET 8.0  
- **docker-compose.yml**: para orquestrar o container único  
- **InvestmentSimulator.Api**: projeto backend (.NET 8.0)  
- **InvestmentSimulator.Web**: projeto frontend (Angular)
- **InvestmentSimulator.Domain**: regras de negócio backend (Angular)
- **InvestmentSimulator.Tests**: projeto de testes (Angular)

---

## Passo a passo no Windows

1. **Abra o PowerShell** (ou CMD) na pasta raiz do projeto.  
2. Verifique se o Docker está ativo:
   docker version
   docker compose version
3. Suba os containers e faça o build completo:
   docker-compose up --build
4. Aguarde até:
   - **Stage 1**: build do Angular (`npm ci` + `ng build`)  
   - **Stage 2**: restore, build e publish da API .NET 8.0  
   - **Stage 3**: criação da imagem runtime  
5. Acesse sua aplicação em:  
   http://localhost:5000


Pronto! Seu backend .NET e seu frontend Angular estarão rodando juntos em um único container.

---

## Executar individualmente os projetos

### Backend (API .NET 8.0)

1. **Pré-requisito**: [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) instalado (mínimo 8.0.x).  
2. Abra o PowerShell (ou CMD) na raiz do repositório e entre na pasta da API:
   cd InvestmentSimulator.Api
3. Restaure os pacotes NuGet:
   dotnet restore
4. Compile e execute em modo Development:
   dotnet run
5. Por padrão a API ficará disponível em  
   http://localhost:5000

### Frontend (Angular)

1. **Pré-requisito**: [Node.js LTS](https://nodejs.org/) instalado (mínimo 16.x ou 18.x).  
2. Abra o PowerShell (ou CMD) na raiz do repositório e entre na pasta do projeto Angular:
   cd InvestmentSimulator.Web
3. Instale as dependências:
   npm ci
4. Rode em modo de desenvolvimento (hot-reload):
   ng serve
5. Acesse no navegador:
   http://localhost:4200

---

## Executar os testes unitários

### Backend (.NET 8.0)

1. **Pré-requisito**: [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.  
2. Abra o PowerShell (ou CMD) na raiz do repositório e entre na pasta do projeto de testes:
   cd InvestmentSimulator.Tests
3. Execute todos os testes da solução:
   dotnet test
