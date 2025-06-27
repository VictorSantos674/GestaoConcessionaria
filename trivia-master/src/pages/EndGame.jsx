import { useState } from 'react';
import { useLocalStorage } from '../hooks/useLocalStorage';
import Ranking from './Ranking';

export default function EndGame({ score, onRestart }) {
  const [name, setName] = useState('');
  const [ranking, setRanking] = useLocalStorage('ranking', []);
  const [saved, setSaved] = useState(false);

  const handleSave = () => {
    if (!name.trim()) return;

    const newRanking = [...ranking, { name, score }];
    newRanking.sort((a, b) => b.score - a.score);
    setRanking(newRanking.slice(0, 10)); // top 10
    setSaved(true);
  };

  return (
    <div className="question-card">
      <h2>🎉 Fim do jogo</h2>
      <p>Sua pontuação: {score}</p>

      {!saved ? (
        <>
          <input
            type="text"
            placeholder="Digite seu nome"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <button onClick={handleSave} disabled={!name.trim()}>
            Salvar Pontuação
          </button>
        </>
      ) : (
        <>
          <Ranking />
          <button onClick={onRestart}>Jogar Novamente</button>
        </>
      )}
    </div>
  );
}
