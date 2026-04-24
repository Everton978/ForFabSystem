# ForFabio — Sistema de Gestão Industrial

> **Indústria 4.0 | Node.js MVC + SQL Server**

---

## 📋 Visão Geral

O **ForFabio** é um sistema web de médio porte para gestão de manufatura que conecta todos os setores da fábrica em um único fluxo digital:

```
Engenharia → Gestor → PCP → Almoxarifado → Produção → Qualidade
```

---

## 🏗️ Arquitetura — Padrão MVC

```
forfabio/
├── scripts/
│   └── initDb.js               ← Script único para criar tabelas no SQL Server
│
├── src/
│   ├── config/
│   │   └── database.js         ← Pool de conexão SQL Server (singleton)
│   │
│   ├── middlewares/
│   │   ├── auth.js             ← Proteção de rotas (sessão)
│   │   └── upload.js           ← Multer — upload de arquivos
│   │
│   ├── models/                 ← Acesso ao banco (queries parametrizadas)
│   │   ├── UserModel.js
│   │   ├── EngenhariaModel.js
│   │   ├── PCPModel.js
│   │   ├── AlmoxarifadoModel.js
│   │   └── QualidadeModel.js   ← Inclui ApontamentoModel
│   │
│   ├── controllers/            ← Lógica de negócio
│   │   ├── AuthController.js
│   │   ├── HomeController.js
│   │   ├── EngenhariaController.js
│   │   ├── PCPController.js
│   │   └── OtherControllers.js ← Producao, Almox, Qualidade, Apontamento, Gestao
│   │
│   ├── routes/
│   │   └── index.js            ← Todas as rotas declaradas
│   │
│   ├── views/                  ← Templates EJS
│   │   ├── partials/
│   │   │   ├── navbar.ejs
│   │   │   └── flash.ejs
│   │   ├── login.ejs
│   │   ├── register.ejs
│   │   ├── home.ejs
│   │   ├── engenharia.ejs
│   │   ├── pcp.ejs
│   │   ├── producao.ejs
│   │   ├── almoxarifado.ejs
│   │   ├── qualidade.ejs
│   │   ├── apontamento.ejs
│   │   ├── gestao.ejs
│   │   ├── 404.ejs
│   │   └── 500.ejs
│   │
│   ├── public/
│   │   ├── css/main.css        ← Estilos globais
│   │   ├── img/                ← Logos
│   │   └── uploads/            ← Arquivos enviados pelos usuários
│   │
│   └── server.js               ← Ponto de entrada da aplicação
│
├── .env.example                ← Modelo de variáveis de ambiente
├── .gitignore
└── package.json
```

---

## 🗄️ Banco de Dados — SQL Server

### Tabelas e Relacionamentos

```
users (RA PK)
  ↓
Apontamentos.RA  →  FK → users.RA
ProducOrder.ResponsavelRA → FK → users.RA
  ↓
TarefasOS.IdOS   → FK → ProducOrder.Id_Op (CASCADE DELETE)
Qualidade.IdOS   → FK → ProducOrder.Id_Op

EngenhariaProjetos (independente, referenciada via URL)
Almoxarifado (independente)
Files (armazenamento binário — uso futuro)
```

### Inicializar o banco

```bash
node scripts/initDb.js
```

---

## 🚀 Como Rodar

### 1. Pré-requisitos

- Node.js >= 18
- SQL Server (local ou remoto) com o banco `ForFabMetalsSystem` criado
- npm

### 2. Instalação

```bash
# Clone / extraia o projeto
cd forfabio

# Instale as dependências
npm install

# Configure as variáveis de ambiente
cp .env.example .env
# Edite o arquivo .env com os dados do seu banco SQL Server
```

### 3. Configuração do .env

```env
PORT=3000
NODE_ENV=development

DB_SERVER=localhost
DB_PORT=1433
DB_DATABASE=ForFabMetalsSystem
DB_USER=sa
DB_PASSWORD=SuaSenhaAqui
DB_ENCRYPT=false
DB_TRUST_SERVER_CERTIFICATE=true

SESSION_SECRET=troque_para_uma_chave_aleatoria_longa
UPLOAD_SIZE_LIMIT_MB=10
```

### 4. Criar as tabelas

```bash
npm run init-db
```

### 5. Iniciar a aplicação

```bash
# Produção
npm start

# Desenvolvimento (com hot-reload)
npm run dev
```

Acesse em: **http://localhost:3000**

---

## 🔐 Segurança

| Recurso | Implementação |
|---------|---------------|
| Senhas | `bcryptjs` com salt rounds = 12 |
| Sessão | `express-session` com cookie `httpOnly` e expiração de 8h |
| SQL Injection | Todas as queries usam **parâmetros nomeados** (mssql input) — zero concatenação |
| Upload | `multer` com whitelist de MIME types e limite de tamanho |
| Rotas protegidas | Middleware `ensureAuthenticated` em todas as rotas privadas |
| Variáveis sensíveis | `dotenv` — nunca commitadas no repositório |

---

## 🌐 Rotas da Aplicação

### Públicas
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/login` | Tela de login |
| POST | `/login` | Autenticar |
| GET | `/cadastro` | Tela de cadastro |
| POST | `/cadastro` | Criar conta |
| GET | `/logout` | Encerrar sessão |

### Protegidas (requer login)
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/home` | Dashboard com KPIs |
| GET/POST | `/engenharia` | Projetos técnicos |
| POST | `/engenharia/:id/revisao` | Corrigir e reenviar |
| GET/POST | `/pcp` | Ordens de Serviço |
| POST | `/pcp/:id/cancelar` | Cancelar OS |
| GET | `/producao` | Fila de produção |
| POST | `/producao/:id/assumir` | Operador assume OS |
| POST | `/producao/:id/status` | Atualizar status |
| POST | `/producao/:id/tarefa` | Marcar tarefa (AJAX) |
| GET/POST | `/almoxarifado` | Estoque de materiais |
| POST | `/almoxarifado/:id/status` | Mudar status item |
| POST | `/almoxarifado/:id/endereco` | Atualizar endereço |
| DELETE | `/almoxarifado/:id` | Remover item |
| GET/POST | `/qualidade` | Laudos de inspeção |
| GET/POST | `/apontamento` | Registro diário |
| GET | `/gestao` | Painel do gestor |
| POST | `/gestao/eng/:id/decidir` | Aprovar/Rejeitar projeto |
| POST | `/gestao/eng/:id/revogar` | Revogar aprovação |
| POST | `/gestao/os/:id/decidir` | Aprovar/Rejeitar OS |

---

## 🔄 Fluxo de Trabalho

```
1. ENGENHARIA → Envia projeto (PDF/imagem/link)
2. GESTOR     → Aprova ou rejeita o projeto
3. PCP        → Cria OS referenciando projeto aprovado
4. GESTOR     → Aprova ou rejeita a OS
5. ALMOXARIFADO → Lança material para a OS aprovada
6. PRODUÇÃO   → Operador assume a OS, executa tarefas, atualiza progresso
7. QUALIDADE  → Registra laudo de inspeção (aprovado/reprovado)
8. APONTAMENTO → Operadores registram horas e desvios do turno
9. GESTOR     → Visualiza tudo: pendências, laudos, desvios, auditoria
```

---

## 📦 Dependências

| Pacote | Uso |
|--------|-----|
| `express` | Framework web |
| `ejs` | Template engine (views) |
| `mssql` | Driver SQL Server |
| `bcryptjs` | Hash de senhas |
| `express-session` | Gerenciamento de sessão |
| `connect-flash` | Mensagens de feedback |
| `multer` | Upload de arquivos |
| `dotenv` | Variáveis de ambiente |
| `nodemon` | Dev: hot-reload |

---

## 🛠️ Troubleshooting

**Erro de conexão com SQL Server:**
- Verifique se o SQL Server Browser está rodando
- Confirme usuário/senha no `.env`
- Se usar Windows Auth, consulte a documentação do `mssql` para `trustedConnection`

**Upload com erro "arquivo muito grande":**
- Aumente `UPLOAD_SIZE_LIMIT_MB` no `.env`

**Página em branco após login:**
- Verifique se as tabelas foram criadas com `npm run init-db`
- Consulte os logs do servidor no terminal

---

*ForFabio Enterprise © 2026 | Desenvolvido com Node.js + SQL Server*
