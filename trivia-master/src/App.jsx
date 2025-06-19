import { useState } from 'react';
import { Routes, Route } from 'react-router-dom';

import ThemeToggle from './components/ThemeToggle';
import Start from './pages/Start';
import Game from './pages/Game';
import EndGame from './pages/EndGame';


function App() {
  const [config, setConfig] = useState(null); // nome, qtd, dificuldade
  const [score, setScore] = useState(0);

  const handleRestart = () => {
    setScore(0);
    setConfig(null);
  };

  return (
    <div>
      <ThemeToggle />
      <Routes>
        <Route path="/" element={<Start setConfig={setConfig} />} />
        <Route
          path="/game"
          element={<Game config={config} setScore={setScore} />}
        />
        <Route
          path="/end"
          element={<EndGame score={score} onRestart={handleRestart} />}
        />
      </Routes>
    </div>
  )
}

export default App
