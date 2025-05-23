# KanbanDashboardCRUD

Sistema de gerenciamento de tarefas no formato Kanban com login de usuário, arrastar e soltar (drag and drop), e integração completa com backend em ASP.NET Core.

⸻

Tecnologias Utilizadas
	•	Frontend
	•	React (Vite)
	•	React Beautiful DnD
	•	Emotion (@emotion/styled para estilos)
	•	Axios (requisições HTTP)
	•	JS-Cookie (cookies de autenticação)
	•	React Router DOM
	•	Backend
	•	.NET 8 + Entity Framework Core
	•	Endpoints RESTful para usuários e tarefas

⸻

Funcionalidades
	•	Autenticação de usuário (Login simples)
	•	Kanban com 3 colunas: Pendente, Em progresso, Concluída
	•	Adicionar, editar, deletar tarefas
	•	Arrastar e soltar tarefas entre colunas
	•	Sincronização em tempo real com o backend
	•	Logout seguro (limpa cookies e estado)
	•	Visual moderno e responsivo

⸻

Como Rodar o Projeto

Pré-requisitos
	•	Node.js (>= 18)
	•	NPM ou Yarn
	•	Backend rodando (.NET API)

1. Clonar o repositório
```
git clone https://github.com/seu-usuario/seu-repo-kanban.git
cd seu-repo-kanban
```

2. Instalar as dependências
```
npm install
# ou
yarn install
```

3. Configurar variáveis de ambiente
```
VITE_API_URL=http://localhost:5261/api
```

4. Rodar o Frontend
```
npm run dev
# ou
yarn dev
```

Rodando o Backend

Certifique-se de já ter rodando sua API ASP.NET Core (dotnet run).
Siga as instruções do README/backend ou documentações internas do backend caso haja necessidade de configurações extras (como migrations, base de dados, etc).

⸻

Fluxo Básico de Uso
	1.	Faça login com um usuário existente.
	2.	Gerencie suas tarefas no kanban (adicionar, mover, editar, excluir).
	3.	Logout remove todos os cookies e reseta o estado do app.

⸻

Sugestões para Produção
	•	Configure HTTPS para produção.
	•	Use variáveis de ambiente para API URL.
	•	Faça deploy do backend e frontend separados (ex: Vercel/Netlify para React, Azure/AWS para .NET).
	•	Proteja suas rotas de frontend/backend.

### Ler documentação específica da API
