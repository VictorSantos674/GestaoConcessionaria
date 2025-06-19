import { useEffect, useState } from 'react';

export default function Timer({ duration, onTimeUp, currentQuestion }) {
  const [timeLeft, setTimeLeft] = useState(duration);

  useEffect(() => {
    setTimeLeft(duration); // reinicia o tempo a cada nova pergunta
  }, [currentQuestion, duration]);

  useEffect(() => {
    if (timeLeft <= 0) {
      onTimeUp();
      return;
    }

    const timer = setInterval(() => {
      setTimeLeft(t => t - 1);
    }, 1000);

    return () => clearInterval(timer);
  }, [timeLeft, onTimeUp]);

  return (
    <div style={{ fontSize: '1.5rem', color: timeLeft <= 5 ? 'red' : 'inherit' }}>
      ⏳ Tempo restante: {timeLeft}s
    </div>
  );
}
