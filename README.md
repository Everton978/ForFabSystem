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



