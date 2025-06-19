import { useEffect, useState } from 'react';
import { fetchQuestions } from '../services/triviaAPI';
import QuestionCard from '../components/QuestionCard';
import Timer from '../components/Timer';
import { useNavigate } from 'react-router-dom';

export default function Game({ config, setScore }) {
  const [questions, setQuestions] = useState([]);
  const [current, setCurrent] = useState(0);
  const [loading, setLoading] = useState(true);
  const [localScore, setLocalScore] = useState(0);
  const navigate = useNavigate();

  useEffect(() => {
    if (!config) return navigate('/');
    fetchQuestions(config.amount, '', config.difficulty).then(data => {
      setQuestions(data);
      setLoading(false);
    });
  }, [config, navigate]);

  const handleAnswer = (isCorrect) => {
    if (isCorrect) setLocalScore(prev => prev + 100);
    setCurrent(prev => prev + 1);
  };

  const handleTimeUp = () => {
    setCurrent(prev => prev + 1);
  };

  useEffect(() => {
    if (current >= questions.length && questions.length > 0) {
      setScore(localScore);
      navigate('/end');
    }
  }, [current, questions, localScore, setScore, navigate]);

  if (loading) return <p>Carregando perguntas...</p>;

  return (
    <div className="question-card">
      <Timer duration={15} onTimeUp={handleTimeUp} currentQuestion={current} />
      <QuestionCard
        question={questions[current]}
        index={current}
        onAnswer={handleAnswer}
      />
      <p>Pontuação: {localScore}</p>
    </div>
  );
}
