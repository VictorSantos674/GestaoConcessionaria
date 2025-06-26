# 🎮 Trivia Master - Quiz Interativo com React


Um quiz interativo desenvolvido com **React**, que desafia o usuário com perguntas de múltipla escolha utilizando a API pública [Open Trivia DB](https://opentdb.com/). Com cronômetro, ranking local, tema escuro/claro e animações suaves, o app oferece uma experiência moderna e dinâmica.

---

## 🚀 Funcionalidades

- ✅ Quiz com perguntas aleatórias da API Open Trivia DB
- ✅ Cronômetro regressivo para cada pergunta
- ✅ Pontuação automática e ranking local com `LocalStorage`
- ✅ Modo escuro/claro com toggle dinâmico
- ✅ Animações com **Framer Motion**
- ✅ Personalização: nome do jogador, dificuldade e número de perguntas
- ✅ Responsivo para dispositivos móveis
- ✅ Interface agradável e intuitiva

---

## 🧪 Tecnologias Utilizadas

| Tecnologia        | Descrição                            |
|-------------------|----------------------------------------|
| ⚛️ React          | Framework principal do front-end       |
| 🎨 CSS Modules    | Estilização com variáveis para temas   |
| 💾 LocalStorage   | Armazenamento do ranking               |
| 🧠 Context API    | Gerenciamento de tema escuro/claro     |
| 🎬 Framer Motion  | Animações suaves entre componentes     |
| 🌐 Open Trivia DB | API de perguntas públicas              |
| 🧭 React Router   | Controle de navegação entre telas      |

---

## 🧰 Instalação e Execução Local

```bash
# Clone o repositório
git clone https://github.com/VictorSantos674/TriviaMaster

# Acesse a pasta
cd trivia-master

# Instale as dependências
npm install

# Inicie o projeto
npm run dev
````

---

## 📦 Estrutura de Pastas

```
src/
│
├── components/          # Componentes reutilizáveis (Timer, QuestionCard...)
├── context/             # Contexto para tema (dark/light)
├── hooks/               # Hooks personalizados (useLocalStorage)
├── pages/               # Telas principais (Start, Game, EndGame, Ranking)
├── services/            # API externa (Open Trivia DB)
├── styles/              # Estilos globais
└── App.jsx              # Definição de rotas e lógica principal
```

---

## 🌐 Deploy

Este projeto pode ser facilmente publicado usando:

* [Vercel](https://vercel.com)
* [GitHub Pages](https://pages.github.com)
* [Netlify](https://netlify.com)

> Basta conectar seu repositório, configurar como app React com `npm run build`, e publicar!

---

## 💡 Melhorias Futuras

* [ ] Login com Firebase e ranking global
* [ ] Suporte a múltiplas categorias
* [ ] Tradução e internacionalização (i18n)
* [ ] Efeitos sonoros e músicas dinâmicas

---

## 🤝 Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

---

## ✨ Desenvolvido por

Victor Souza 🚀
[LinkedIn](https://www.linkedin.com/in/vicsantosdev/) • [GitHub](https://github.com/VictorSantos674)