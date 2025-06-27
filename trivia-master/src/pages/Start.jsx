import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Start({ setConfig }) {
  const [name, setName] = useState('');
  const [difficulty, setDifficulty] = useState('medium');
  const [amount, setAmount] = useState(5);
  const navigate = useNavigate();

  const startGame = () => {
    if (name.trim().length < 2) {
      return alert('Digite um nome válido!');
    }
    setConfig({ name, difficulty, amount });
    navigate('/game');
  };

  return (
    <div className="question-card">
      <h1>🎮 Trivia Master</h1>

      <label htmlFor="name">Seu nome:</label>
      <input
        id="name"
        type="text"
        placeholder="Digite seu nome"
        value={name}
        onChange={(e) => setName(e.target.value)}
      />

      <label htmlFor="difficulty">Dificuldade:</label>
      <select
        id="difficulty"
        value={difficulty}
        onChange={(e) => setDifficulty(e.target.value)}
      >
        <option value="easy">Fácil</option>
        <option value="medium">Médio</option>
        <option value="hard">Difícil</option>
      </select>

      <label htmlFor="amount">Quantidade de Perguntas:</label>
      <input
        id="amount"
        type="number"
        min="1"
        max="50"
        value={amount}
        onChange={(e) => setAmount(Number(e.target.value))}
      />

      <button onClick={startGame}>Começar Jogo</button>
    </div>
  );
}
