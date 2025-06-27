import { useNavigate } from 'react-router-dom';
import ThemeToggle from '../components/ThemeToggle';

export default function Home() {
  const navigate = useNavigate();

  return (
    <div className="question-card">
      <ThemeToggle />

      <h1>🧠 Trivia Master</h1>
      <p>Teste seus conhecimentos com perguntas desafiadoras!</p>

      <button onClick={() => navigate('/start')}>
        🎮 Iniciar Jogo
      </button>

      <button onClick={() => navigate('/ranking')}>
        🏆 Ver Ranking
      </button>
    </div>
  );
}
