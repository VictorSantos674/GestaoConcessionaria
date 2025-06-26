import { useEffect, useState } from 'react';
import { shuffleArray } from '../utils/shuffleArray';
import { motion } from 'framer-motion';
import he from 'he'; // opcional, para evitar dangerouslySetInnerHTML

export default function QuestionCard({ question, index, onAnswer }) {
  const [answers, setAnswers] = useState([]);
  const [selected, setSelected] = useState(null);
  const [isCorrect, setIsCorrect] = useState(null);

  useEffect(() => {
    const shuffled = shuffleArray([
      ...question.incorrect_answers,
      question.correct_answer
    ]);
    setAnswers(shuffled);
    setSelected(null);
    setIsCorrect(null);
  }, [question]);

  const handleSelect = (answer) => {
    if (selected) return; // evita múltiplos cliques

    setSelected(answer);
    const correct = answer === question.correct_answer;
    setIsCorrect(correct);

    setTimeout(() => {
      onAnswer(correct);
    }, 1500);
  };

  return (
    <motion.div
      className="question-card"
      initial={{ opacity: 0, y: 30 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.4 }}
    >
      <h2>{he.decode(question.question)}</h2>
      <ul>
        {answers.map((answer) => {
          let className = '';
          if (selected) {
            if (answer === question.correct_answer) className = 'correct';
            else if (answer === selected) className = 'incorrect';
          }

          return (
            <li key={answer}>
              <button
                className={className}
                onClick={() => handleSelect(answer)}
                disabled={!!selected}
                aria-pressed={selected === answer}
                aria-label={`Opção: ${he.decode(answer)}`}
              >
                {he.decode(answer)}
              </button>
            </li>
          );
        })}
      </ul>
      <p>Pergunta {index + 1}</p>
    </motion.div>
  );
}
