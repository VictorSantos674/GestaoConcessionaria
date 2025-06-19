import { useState } from 'react';
import { useLocalStorage } from '../hooks/useLocalStorage';

export default function Ranking({ onBack }) {
  const [ranking, setRanking] = useLocalStorage('ranking', []);

  const handleClear = () => {
    if (window.confirm('Deseja realmente limpar o ranking?')) {
      setRanking([]);
    }
  };

  return (
    <div className="question-card">
      <h2>🏆 Ranking dos Melhores Jogadores</h2>

      {ranking.length === 0 ? (
        <p>Nenhum registro encontrado.</p>
      ) : (
        <ol>
          {ranking.map(({ name, score }, index) => (
            <li key={index}>
              {name} — {score} pts
            </li>
          ))}
        </ol>
      )}

      <button onClick={handleClear} style={{ marginRight: 10 }}>
        Limpar Ranking
      </button>

      {onBack && (
        <button onClick={onBack}>
          Voltar
        </button>
      )}
    </div>
  );
}
