
# 🚗 Gestão de Concessionárias de Veículos

Sistema web completo para gestão de concessionárias, fabricantes, veículos, vendas e relatórios, desenvolvido em **ASP.NET MVC** com **Entity Framework**. O projeto atende a requisitos reais de autenticação, autorização, integração com APIs externas, dashboards e otimização de desempenho.

---

## 📋 Funcionalidades Principais

- **Autenticação e autorização de usuários** (Administrador, Gerente, Vendedor)
- **CRUD completo** para Fabricantes, Veículos, Concessionárias, Clientes e Vendas
- **Validações avançadas** (unicidade, formatos, datas, CPF, etc.)
- **Deleção lógica** de registros
- **Integração AJAX** para carregamento dinâmico de veículos e consulta de CEP
- **Geração de protocolo único** para vendas
- **Relatórios mensais** e dashboards com gráficos (Chart.js)
- **Exportação de relatórios** para PDF/Excel
- **Caching** e otimizações de desempenho
- **Testes unitários/integrados** e documentação técnica

---

## 🏗️ Modelagem de Dados

- **Fabricantes:** Nome único, país de origem, ano de fundação, website
- **Veículos:** Modelo, ano, preço, fabricante, tipo, descrição
- **Concessionárias:** Nome único, endereço completo, cidade, estado, CEP, telefone, e-mail, capacidade máxima
- **Clientes:** Nome, CPF (único), telefone
- **Vendas:** Veículo, concessionária, cliente, data, preço, protocolo único

---

## 🧑‍💼 Perfis de Usuário

- **Administrador:** Gerencia fabricantes e concessionárias
- **Gerente:** Gerencia veículos, relatórios e dashboards
- **Vendedor:** Realiza vendas e cadastra clientes

---

## 🚦 Casos de Uso

1. **Cadastro de Fabricante** (Admin)
2. **Cadastro de Veículo** (Gerente)
3. **Cadastro de Concessionária** (Admin)
4. **Realização de Venda** (Vendedor)
5. **Geração de Relatórios** (Gerente)
6. **Autenticação de Usuários** (Todos)

---

## 🚀 Tecnologias Utilizadas

| Tecnologia         | Descrição                                 |
|--------------------|-------------------------------------------|
| ASP.NET MVC        | Backend e lógica de negócio               |
| Entity Framework   | ORM e persistência de dados               |
| Identity Framework | Autenticação e autorização                |
| Bootstrap          | Frontend responsivo                       |
| JavaScript/AJAX    | Interatividade e integração com APIs      |
| Chart.js           | Dashboards e gráficos                     |
| SQL Server/LocalDB | Banco de dados relacional                 |
| Redis/Memcached    | Caching e otimização                      |
| Swagger            | Documentação de API                       |

---

## 🧰 Instalação e Execução

```bash
# Clone o repositório
git clone https://github.com/VictorSantos674/GestaoConcessionaria

# Acesse a pasta do projeto
cd GestaoConcessionaria

# Restaure os pacotes NuGet
dotnet restore

# Execute as migrations para criar o banco de dados
dotnet ef database update

# Inicie o projeto
dotnet run --project src/DesafioIntelectah
```

---

## 📦 Estrutura de Pastas

```
src/
├── Controllers/         # Controllers MVC (CRUD, autenticação, vendas)
├── Data/                # Contexto do Entity Framework e Migrations
├── Models/              # Modelos de domínio (Fabricante, Veículo, etc.)
├── ViewModels/          # ViewModels para telas e relatórios
├── Views/               # Views Razor (CRUD, vendas, relatórios)
├── root/                # Arquivos estáticos (css, js, lib)
└── appsettings.json     # Configurações do projeto
```

---

## 📊 Relatórios e Dashboards

- Relatórios mensais de vendas por tipo, fabricante e concessionária
- Dashboards com gráficos interativos (Chart.js)
- Exportação para PDF/Excel

---

## 🔒 Segurança

- Rotas protegidas por perfil de usuário ([Authorize])
- Senhas criptografadas e autenticação robusta
- Validação de dados em todas as camadas

---

## 💡 Melhorias Futuras

- [ ] Integração com APIs automotivas externas
- [ ] Otimização avançada de queries e lazy loading
- [ ] Testes automatizados e cobertura total
- [ ] Documentação técnica detalhada (Swagger)

---

## 🤝 Contribuição

Contribuições são bem-vindas! Abra issues ou pull requests para sugerir melhorias.

---

## 📄 Licença

Este projeto está sob a licença MIT.

---

## ✨ Desenvolvido por

Victor Souza �
[LinkedIn](https://www.linkedin.com/in/vicsantosdev/) • [GitHub](https://github.com/VictorSantos674)