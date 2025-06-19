import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Start({ setConfig }) {
  const [name, setName] = useState('');
  const [difficulty, setDifficulty] = useState('medium');
  const [amount, setAmount] = useState(5);
  const navigate = useNavigate();

  const startGame = () => {
    if (!name) return alert('Digite seu nome!');
    setConfig({ name, difficulty, amount });
    navigate('/game');
  };

  return (
    <div className="question-card">
      <h1>🎮 Trivia Master</h1>

      <input
        type="text"
        placeholder="Seu nome"
        value={name}
        onChange={(e) => setName(e.target.value)}
      />

      <label>Dificuldade:</label>
      <select
        value={difficulty}
        onChange={(e) => setDifficulty(e.target.value)}
      >
        <option value="easy">Fácil</option>
        <option value="medium">Médio</option>
        <option value="hard">Difícil</option>
      </select>

      <label>Quantidade de Perguntas:</label>
      <input
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
